using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace API.SignalR
{
    [Authorize]
    public class MessageHub : Hub
    {
        private readonly IMapper _mapper;
        private readonly IHubContext<PresenceHub> _presenceHub;
        private readonly IUnitOfWork _uow;
        private readonly IUserRepository _userRepository;
        private readonly IMessageRepository _messageRepository;

        public MessageHub(IUnitOfWork uow, IMapper mapper, IHubContext<PresenceHub> presenceHub, IUserRepository userRepository, IMessageRepository messageRepository)
        {
            _uow = uow;
            _presenceHub = presenceHub;
            _mapper = mapper;
            _userRepository = userRepository;
            _messageRepository = messageRepository;
        }

        public override async Task OnConnectedAsync()
        {
            var username = Context.User.FindFirst(ClaimTypes.Name)?.Value;

            var httpContext = Context.GetHttpContext();
            var otherUser = httpContext.Request.Query["user"];

            var groupName = GetGroupName(username, otherUser);

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            var group = await AddToGroup(groupName);

            await Clients.Group(groupName).SendAsync("UpdatedGroup", group);

            var messages = await _messageRepository.GetMessageThread(username, otherUser);

            await _uow.SaveChangesAsync();

            await Clients.Caller.SendAsync("ReceiveMessageThread", messages);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var group = await RemoveFromMessageGroup();
            await Clients.Group(group.Name).SendAsync("UpdatedGroup");
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(CreateMessageDto createMessageDto)
        {
            var username = Context.User.FindFirst(ClaimTypes.Name)?.Value;

            if (username == createMessageDto.RecipientUserName.ToLower())
            {
                throw new HubException("You cannot send messages to yourself");
            }

            var sender = await _userRepository.GetUserByUserNameAsync(username);
            var recipient = await _userRepository.GetUserByUserNameAsync(createMessageDto.RecipientUserName);

            if (recipient == null) throw new HubException("User not found");

            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUserName = sender.UserName,
                RecipientUserName = recipient.UserName,
                Content = createMessageDto.Content
            };

            var groupName = GetGroupName(sender.UserName, recipient.UserName);
            var group = await _messageRepository.GetMessageGroup(groupName);

            if (group.Connections.Any(x => x.Username == recipient.UserName))
            {
                message.DateRead = DateTime.UtcNow;
            }
            else
            {
                var connections = await PresenceTracker.GetConnectionForUser(recipient.UserName);
                if (connections != null)
                {
                    await _presenceHub.Clients.Clients(connections).SendAsync("NewMessageReceived",
                        new { username = sender.UserName, knownAs = sender.KnownAs });
                }
            }

            _messageRepository.AddMessage(message);

            if (await _uow.SaveChangesAsync() > 0)
            {
                await Clients.Group(groupName).SendAsync("NewMessage", _mapper.Map<MessageDto>(message));

            }

        }
        private string GetGroupName(string caller, string other)
        {
            var stringCompare = string.CompareOrdinal(caller, other) < 0;
            return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
        }

        private async Task<Group> AddToGroup(string groupName)
        {
            var username = Context.User.FindFirst(ClaimTypes.Name)?.Value;
            var group = await _messageRepository.GetMessageGroup(groupName);
            var connection = new Connection(Context.ConnectionId, username);

            if (group == null)
            {
                group = new Group(groupName);
                _messageRepository.AddGroup(group);
            }

            group.Connections.Add(connection);

            if (await _uow.SaveChangesAsync() > 0) return group;

            throw new HubException("Failed to add to group");
        }

        private async Task<Group> RemoveFromMessageGroup()
        {
            var group = await _messageRepository.GetGroupForConnection(Context.ConnectionId);
            var connection = group.Connections.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);

            _messageRepository.RemoveConnection(connection);
            if (await _uow.SaveChangesAsync() > 0) return group;
            throw new HubException("Failed to remove form, group");
        }
    }
}
