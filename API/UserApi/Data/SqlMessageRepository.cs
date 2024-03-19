using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using UserAPI.DTOs;
using UserAPI.Entities;
using UserAPI.Helpers;
using UserAPI.Interfaces;

namespace UserAPI.Data
{
    public class SqlMessageRepository : IMessageRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public SqlMessageRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public Task AddConnectionAsync(NewConnection connection)
        {
            throw new NotImplementedException();
        }

        public void AddGroup(Group group)
        {
            _context.Groups.Add(group);
        }

        public Task AddGroupAsync(NewGroup group)
        {
            throw new NotImplementedException();
        }

        public void AddMessage(Message message)
        {
            _context.Messages.Add(message);
        }

        public Task AddMessageAsync(NewMessage newMessage)
        {
            throw new NotImplementedException();
        }

        public void DeleteMessage(Message message)
        {
            _context.Messages.Remove(message);
        }

        public Task DeleteMessageAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<Connection> GetConnection(string connectionId)
        {
            return await _context.Connections.FindAsync(connectionId);
        }

        public Task<NewConnection> GetConnectionAsync(string connectionId)
        {
            throw new NotImplementedException();
        }

        public Task<NewConnection> GetConnectionByIdAsync(string Id)
        {
            throw new NotImplementedException();
        }

        public async Task<Group> GetGroupForConnection(string connectionId)
        {
            return await _context.Groups
                .Include(x => x.Connections)
                .Where(x => x.Connections.Any(c => c.ConnectionId == connectionId))
                .FirstOrDefaultAsync();
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages.FindAsync(id);
        }

        public async Task<Group> GetMessageGroup(string groupName)
        {
            return await _context.Groups
                .Include(x => x.Connections)
                .FirstOrDefaultAsync(x => x.Name == groupName);
        }

        public Task<NewGroup> GetMessageGroupAsync(string groupName)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams)
        {
            var query = _context.Messages.OrderByDescending(x => x.MessageSent).AsQueryable();
            query = messageParams.Container switch
            {
                "Inbox" => query.Where(u => u.RecipientUserName == messageParams.Username && u.RecipientDeleted == false),
                "Outbox" => query.Where(u => u.SenderUserName == messageParams.Username && u.SenderDeleted == false),
                _ => query.Where(u => u.RecipientUserName == messageParams.Username && u.DateRead == null && u.RecipientDeleted == false)
            };

            var messages = query.ProjectTo<MessageDto>(_mapper.ConfigurationProvider);
            return await PagedList<MessageDto>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<IEnumerable<MessageDto>> GetMessageThread(string currentUserName, string recipientUserName)
        {
            var messages = await _context.Messages
                .Include(u => u.Sender).ThenInclude(p => p.Photos)
                .Include(u => u.Recipient).ThenInclude(p => p.Photos)
                .Where(
                    m => m.RecipientUserName == currentUserName && m.RecipientDeleted == false && m.SenderUserName == recipientUserName ||
                         m.RecipientUserName == recipientUserName && m.SenderDeleted == false && m.SenderUserName == currentUserName)
                .OrderBy(m => m.MessageSent).ToListAsync();

            var unreadMessages = messages.Where(m => m.DateRead == null
                                                     && m.RecipientUserName == currentUserName).ToList();

            if (unreadMessages.Any())
            {
                foreach (var message in unreadMessages)
                {
                    message.DateRead = DateTime.UtcNow;
                }

            }

            return _mapper.Map<IEnumerable<MessageDto>>(messages);
        }

        public void RemoveConnection(Connection connection)
        {
            _context.Connections.Remove(connection);
        }

        public Task<MongoDB.Driver.DeleteResult> RemoveConnectionAsync(NewConnection connection)
        {
            throw new NotImplementedException();
        }

        public Task UpdateMessageAsync(MessageDto newMessage)
        {
            throw new NotImplementedException();
        }
    }
}
