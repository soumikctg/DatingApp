using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using MediatR;
using MessageAPI.Commands;
using MessageAPI.Interfaces;
using MessageAPI.DTOs;
using MessageAPI.Models;
using MessageAPI.Queries;

namespace MessageAPI.Hubs;

[Authorize]
public class MessageHub : Hub
{
    private readonly IMapper _mapper;
    private readonly IMessageRepository _messageRepository;
    private readonly IMediator _mediator;

    public MessageHub(IMapper mapper, IMessageRepository messageRepository, IMediator mediator)
    {
        _mediator = mediator;
        _mapper = mapper;
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

        var messageThread = new GetMessageThreadQuery
        {
            UserName = otherUser,
            CurrentUserName = username
        };

        var messages = await _mediator.Send(messageThread);


        foreach (var message in messages)
        {
            if (message.RecipientUserName == username && message.DateRead == null)
            {
                message.DateRead = DateTime.UtcNow;
                var updateMessage = new UpdateMessageCommand
                {
                    Message = message
                };
                await _mediator.Send(updateMessage);
            }

        }


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

        var message = new Message()
        {
            SenderUserName = sender.UserName,
            SenderPhotoUrl = sender.Photos.FirstOrDefault(x => x.IsMain)?.Url,
            RecipientUserName = recipient.UserName,
            RecipientPhotoUrl = recipient.Photos.FirstOrDefault(x => x.IsMain)?.Url,
            Content = createMessageDto.Content
        };

        var groupName = GetGroupName(sender.UserName, recipient.UserName);

        var ConnectionQuery = new GetConnectionQuery
        {
            UserName = recipient.UserName
        };

        var connection = await _mediator.Send(ConnectionQuery);
        if (connection != null)
        {
            message.DateRead = DateTime.UtcNow;
        }
        /*else
        {
            var connections = await PresenceTracker.GetConnectionForUser(recipient.UserName);
            if (connections != null)
            {
                await _presenceHub.Clients.Clients(connections).SendAsync("NewMessageReceived",
                    new { username = sender.UserName, knownAs = sender.KnownAs });
            }
        }*/

        await _messageRepository.AddMessageAsync(message);


        await Clients.Group(groupName).SendAsync("NewMessage", _mapper.Map<MessageDto>(message));

    }
    private string GetGroupName(string caller, string other)
    {
        var stringCompare = string.CompareOrdinal(caller, other) < 0;
        return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
    }

    private async Task<Group> AddToGroup(string groupName)
    {
        var username = Context.User.FindFirst(ClaimTypes.Name)?.Value;
        var group = await _messageRepository.GetMessageGroupAsync(groupName);
        var connection = new Connection(Context.ConnectionId, username, groupName);

        if (group == null)
        {
            group = new Group(groupName);
            await _messageRepository.AddGroupAsync(group);
        }

        await _messageRepository.AddConnectionAsync(connection);

        return group;
    }

    private async Task<Group> RemoveFromMessageGroup()
    {

        var connection = await _messageRepository.GetConnectionByIdAsync(Context.ConnectionId);
        await _messageRepository.RemoveConnectionAsync(connection);

        var group = await _messageRepository.GetMessageGroupAsync(connection.GroupName);
        return group;

        /*if (await _uow.SaveChangesAsync() > 0) return group;*/
        /*throw new HubException("Failed to remove form, group");*/
    }
}