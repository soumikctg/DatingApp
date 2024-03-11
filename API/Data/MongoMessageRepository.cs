using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using MongoDB.Driver;

namespace API.Data
{
    public class MongoMessageRepository : IMessageRepository
    {
        private readonly List<Message> _messages = new List<Message>();
        private readonly IMongoClient _mongoClient;

        public MongoMessageRepository(IConfiguration configuration)
        {
            var conString = configuration["DatabaseConfig:ConnectionString"];
            _mongoClient = new MongoClient(conString);
        }

        private IMongoCollection<Message> GetMongoCollection()
        {
            return _mongoClient.GetDatabase("DatingAppDb").GetCollection<Message>("Message");
        }

        public void AddGroup(Group group)
        {
            throw new NotImplementedException();
        }

        public void AddMessage(Message message)
        {
            GetMongoCollection().InsertOne(message);  // no caching.. 
        }

        public void DeleteMessage(Message message)
        {
            GetMongoCollection()
                .DeleteOne(x => x.Id == message.Id);
        }

        public Task<Connection> GetConnection(string connectionId)
        {
            throw new NotImplementedException();
        }

        public Task<Group> GetGroupForConnection(string connectionId)
        {
            throw new NotImplementedException();
        }

        public async Task<Message> GetMessage(int id)
        {
            var cursor = await GetMongoCollection().FindAsync(x => x.Id == id);

            return cursor.FirstOrDefault();
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
