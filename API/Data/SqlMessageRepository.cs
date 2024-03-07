using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
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

        public void AddGroup(Group group)
        {
            _context.Groups.Add(group);
        }

        public void AddMessage(Message message)
        {
            _context.Messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            _context.Messages.Remove(message);
        }

        public async Task<Connection> GetConnection(string connectionId)
        {
            return await _context.Connections.FindAsync(connectionId);
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

                await _context.SaveChangesAsync();
            }

            return _mapper.Map<IEnumerable<MessageDto>>(messages);
        }

        public void RemoveConnection(Connection connection)
        {
            _context.Connections.Remove(connection);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }

    public class MongoMessageRepository : IMessageRepository
    {
        private readonly List<Message> _messages = new List<Message>();

        public MongoMessageRepository(IConfiguration configuration)
        {
            var conString = configuration["DatabaseConfig:ConnectionString"];
        }

        public void AddGroup(Group group)
        {
            throw new NotImplementedException();
        }

        public void AddMessage(Message message)
        {
            _messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            _messages.Remove(message);
        }

        public Task<Connection> GetConnection(string connectionId)
        {
            throw new NotImplementedException();
        }

        public async Task<Message> GetMessage(int id)
        {
            return _messages.First(x => x.Id == id);
        }

        public Task<Group> GetMessageGroup(string groupName)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams)
        {
            return await Task.FromResult(new PagedList<MessageDto>(new List<MessageDto>(), 100, 1, 10));
        }

        public async Task<IEnumerable<MessageDto>> GetMessageThread(string currentUserName, string recipientUserName)
        {
            return await Task.FromResult(new List<MessageDto>());
        }

        public void RemoveConnection(Connection connection)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await Task.FromResult(true);
        }
    }
}
