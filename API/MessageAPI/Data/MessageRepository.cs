using AutoMapper;
using MessageAPI.DTOs;
using MessageAPI.Helpers;
using MessageAPI.Interfaces;
using MessageAPI.Models;
using MongoDB.Driver;

namespace MessageAPI.Data;

public class MessageRepository : IMessageRepository
{

    private readonly IMongoClient _mongoClient;
    private readonly IMapper _mapper;

    public MessageRepository(IConfiguration configuration, IMongoClientProvider mongoClientProvider, IMapper mapper)
    {
        _mapper = mapper;
        var connectionString = configuration["MongoDbConfig:ConnectionString"];
        _mongoClient = mongoClientProvider.GetClient(connectionString);
    }


    private IMongoCollection<Message> GetMongoCollectionForMessages()
    {
        return _mongoClient.GetDatabase("DatingAppDb").GetCollection<Message>("Messages");
    }

    private IMongoCollection<Connection> GetMongoCollectionForConnection()
    {
        return _mongoClient.GetDatabase("DatingAppDb").GetCollection<Connection>("Connection");
    }
    private IMongoCollection<Group> GetMongoCollectionForGroup()
    {
        return _mongoClient.GetDatabase("DatingAppDb").GetCollection<Group>("Group");
    }






    public async Task AddMessageAsync(Message newMessage)
    {
        await GetMongoCollectionForMessages().InsertOneAsync(newMessage); // no caching.. 
    }
    public async Task UpdateMessageAsync(MessageDto newMessage)
    {
        var filter = Builders<Message>.Filter.Eq(x => x.Id, newMessage.Id);
        var update = Builders<Message>.Update.Set(x => x.DateRead, newMessage.DateRead);

        await GetMongoCollectionForMessages().UpdateOneAsync(filter, update);
    }
    public Task GetMessageByIdAsync(string id)
    {
        throw new NotImplementedException();
    }
    public async Task DeleteMessageAsync(string id)
    {
        await GetMongoCollectionForMessages().DeleteOneAsync(x => x.Id == id);
    }
    public async Task AddGroupAsync(Group group)
    {
        await GetMongoCollectionForGroup().InsertOneAsync(group);
    }
    public async Task AddConnectionAsync(Connection connection)
    {
        await GetMongoCollectionForConnection().InsertOneAsync(connection);
    }
    public async Task<Connection> GetConnectionAsync(string username)
    {
        var filter = Builders<Connection>.Filter.Eq(x => x.Username, username);
        return await GetMongoCollectionForConnection().Find(filter).FirstOrDefaultAsync();
    }

    public async Task<Connection> GetConnectionByIdAsync(string id)
    {
        var filter = Builders<Connection>.Filter.Eq(x => x.ConnectionId, id);
        return await GetMongoCollectionForConnection().Find(filter).FirstOrDefaultAsync();
    }

    public async Task<DeleteResult> RemoveConnectionAsync(Connection connection)
    {
        var filter = Builders<Connection>.Filter.Eq(x => x.Id, connection.Id);
        var result = await GetMongoCollectionForConnection().DeleteOneAsync(filter);
        return result;
    }


    public async Task<Group> GetMessageGroupAsync(string groupName)
    {
        var filter = Builders<Models.Group>.Filter.Eq(x => x.Name, groupName);

        return (await GetMongoCollectionForGroup().FindAsync(filter)).FirstOrDefault();
    }

    public async Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams)
    {
        var recipientFilter1 = Builders<Message>.Filter.Eq(x => x.RecipientUserName, messageParams.Username);
        var deleteFilter1 = Builders<Message>.Filter.Eq(x => x.RecipientDeleted, false);
        var senderFilter1 = Builders<Message>.Filter.Eq(x => x.SenderUserName, messageParams.Username);
        var deleteFilter2 = Builders<Message>.Filter.Eq(x => x.SenderDeleted, false);
        var unreadFilter = Builders<Message>.Filter.Eq(x => x.DateRead, null);
        var order = Builders<Message>.Sort.Descending(x => x.MessageSent);
        FilterDefinition<Message> filter;
        if (messageParams.Container == "Inbox")
        {
            filter = recipientFilter1 & deleteFilter1;
        }
        else if (messageParams.Container == "Outbox")
        {
            filter = senderFilter1 & deleteFilter2;
        }
        else
        {
            filter = recipientFilter1 & unreadFilter & deleteFilter1;
        }

        var messages = await GetMongoCollectionForMessages().Find(filter).Sort(order).Skip((messageParams.PageNumber - 1) * messageParams.PageSize)
            .Limit(messageParams.PageSize).ToListAsync();
        var count = await GetMongoCollectionForMessages().CountDocumentsAsync(filter);
        var messageDto = _mapper.Map<List<MessageDto>>(messages);
        var pagedMessages =
            new PagedList<MessageDto>(messageDto, count, messageParams.PageNumber, messageParams.PageSize);
        return pagedMessages;
    }

    
    public async Task<IEnumerable<MessageDto>> GetMessageThread(string currentUserName, string recipientUserName)
    {

        var recipientFilter1 = Builders<Message>.Filter.Eq(x => x.RecipientUserName, currentUserName);
        var deleteFilter1 = Builders<Message>.Filter.Eq(x => x.RecipientDeleted, false);
        var senderFilter1 = Builders<Message>.Filter.Eq(x => x.SenderUserName, recipientUserName);
        var recipientFilter2 = Builders<Message>.Filter.Eq(x => x.RecipientUserName, recipientUserName);
        var deleteFilter2 = Builders<Message>.Filter.Eq(x => x.SenderDeleted, false);
        var senderFilter2 = Builders<Message>.Filter.Eq(x => x.SenderUserName, currentUserName);

        var filter = recipientFilter1 & deleteFilter1 & senderFilter1 |
                     recipientFilter2 & deleteFilter2 & senderFilter2;

        var unreadFilter = Builders<Message>.Filter.Eq(x => x.DateRead, null);

        var filter2 = unreadFilter & recipientFilter1;

        var order = Builders<Message>.Sort.Ascending(x => x.MessageSent);

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


}