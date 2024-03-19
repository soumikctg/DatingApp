using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using UserAPI.DTOs;
using UserAPI.Entities;
using UserAPI.Helpers;
using UserAPI.Interfaces;

namespace UserAPI.Data.MongoRepository
{
    public class MongoMessageRepository : IMessageRepository
    {
        private readonly List<Message> _messages = new List<Message>();
        private readonly IMongoClient _mongoClient;
        private readonly IMapper _mapper;

        public MongoMessageRepository(IConfiguration configuration, IMongoClientProvider mongoClientProvider, IMapper mapper)
        {
            _mapper = mapper;
            var connectionString = configuration["MongoDbConfig:ConnectionString"];
            _mongoClient = mongoClientProvider.GetClient(connectionString);
        }

        private IMongoCollection<NewMessage> GetMongoCollectionForMessages()
        {
            return _mongoClient.GetDatabase("DatingAppDb").GetCollection<NewMessage>("Messages");
        }

        private IMongoCollection<NewConnection> GetMongoCollectionForConnection()
        {
            return _mongoClient.GetDatabase("DatingAppDb").GetCollection<NewConnection>("Connection");
        }
        private IMongoCollection<NewGroup> GetMongoCollectionForGroup()
        {
            return _mongoClient.GetDatabase("DatingAppDb").GetCollection<NewGroup>("Group");
        }

        public async Task AddGroupAsync(NewGroup group)
        {
            await GetMongoCollectionForGroup().InsertOneAsync(group);
        }

        public async Task AddMessageAsync(NewMessage newMessage)
        {
            await GetMongoCollectionForMessages().InsertOneAsync(newMessage); // no caching.. 
        }

        public async Task UpdateMessageAsync(MessageDto newMessage)
        {
            var filter = Builders<NewMessage>.Filter.Eq(x => x.Id, newMessage.Id);
            var update = Builders<NewMessage>.Update.Set(x => x.DateRead, newMessage.DateRead);

            await GetMongoCollectionForMessages().UpdateOneAsync(filter, update);
        }

        public async Task DeleteMessageAsync(string id)
        {
            await GetMongoCollectionForMessages().DeleteOneAsync(x => x.Id == id);
        }

        public void DeleteMessage(Message message)
        {


            throw new NotImplementedException();
        }

        public Task<Connection> GetConnection(string connectionId)
        {
            throw new NotImplementedException();
        }

        public async Task AddConnectionAsync(NewConnection connection)
        {
            await GetMongoCollectionForConnection().InsertOneAsync(connection);
        }

        public async Task<NewConnection> GetConnectionAsync(string username)
        {
            var filter = Builders<NewConnection>.Filter.Eq(x => x.Username, username);
            return await GetMongoCollectionForConnection().Find(filter).FirstOrDefaultAsync();
        }

        public async Task<NewConnection> GetConnectionByIdAsync(string id)
        {
            var filter = Builders<NewConnection>.Filter.Eq(x => x.ConnectionId, id);
            return await GetMongoCollectionForConnection().Find(filter).FirstOrDefaultAsync();
        }

        public async Task<DeleteResult> RemoveConnectionAsync(NewConnection connection)
        {
            var filter = Builders<NewConnection>.Filter.Eq(x => x.Id, connection.Id);
            var result = await GetMongoCollectionForConnection().DeleteOneAsync(filter);
            return result;
        }

        public Task<Group> GetGroupForConnection(string connectionId)
        {
            throw new NotImplementedException();
        }

        public async Task<Message> GetMessage(int id)
        {
            /*var cursor = await GetMongoCollection().FindAsync(x => x.Id == id);

            return cursor.FirstOrDefault();*/
            throw new NotImplementedException();
        }

        public Task<Group> GetMessageGroup(string groupName)
        {
            throw new NotImplementedException();

        }

        public async Task<NewGroup> GetMessageGroupAsync(string groupName)
        {
            var filter = Builders<NewGroup>.Filter.Eq(x => x.Name, groupName);

            return (await GetMongoCollectionForGroup().FindAsync(filter)).FirstOrDefault();
        }

        public async Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams)
        {
            return await Task.FromResult(new PagedList<MessageDto>(new List<MessageDto>(), 100, 1, 10));
        }

        public async Task<IEnumerable<MessageDto>> GetMessageThread(string currentUserName, string recipientUserName)
        {

            var recipientFilter1 = Builders<NewMessage>.Filter.Eq(x => x.RecipientUserName, currentUserName);
            var deleteFilter1 = Builders<NewMessage>.Filter.Eq(x => x.RecipientDeleted, false);
            var senderFilter1 = Builders<NewMessage>.Filter.Eq(x => x.SenderUserName, recipientUserName);
            var recipientFilter2 = Builders<NewMessage>.Filter.Eq(x => x.RecipientUserName, recipientUserName);
            var deleteFilter2 = Builders<NewMessage>.Filter.Eq(x => x.SenderDeleted, false);
            var senderFilter2 = Builders<NewMessage>.Filter.Eq(x => x.SenderUserName, currentUserName);

            var filter = recipientFilter1 & deleteFilter1 & senderFilter1 |
                         recipientFilter2 & deleteFilter2 & senderFilter2;

            var unreadFilter = Builders<NewMessage>.Filter.Eq(x => x.DateRead, null);

            var filter2 = unreadFilter & recipientFilter1;

            var order = Builders<NewMessage>.Sort.Ascending(x => x.MessageSent);

            var messages = await GetMongoCollectionForMessages().Find(filter).Sort(order).ToListAsync();


            var unreadMessages = await GetMongoCollectionForMessages().Find(filter2).ToListAsync();

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
            throw new NotImplementedException();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await Task.FromResult(true);
        }

        void IMessageRepository.AddMessage(Message message)
        {
            throw new NotImplementedException();
        }

        public void AddMessage(Message message)
        {
            throw new NotImplementedException();
        }


        public void AddGroup(Group group)
        {
            throw new NotImplementedException();
        }


    }
}
