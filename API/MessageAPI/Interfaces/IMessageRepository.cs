using MessageAPI.DTOs;
using MessageAPI.Helpers;
using MessageAPI.Models;
using MongoDB.Driver;

namespace MessageAPI.Interfaces;

public interface IMessageRepository
{
    Task AddMessageAsync(Message newMessage);
    Task UpdateMessageAsync(MessageDto newMessage);
    Task GetMessageByIdAsync(string id);
    Task DeleteMessageAsync(string id);

    Task AddGroupAsync(Group group);

    
    Task AddConnectionAsync(Connection connection);
    Task<Connection> GetConnectionAsync(string connectionId);
    Task<Connection> GetConnectionByIdAsync(string Id);
    Task<DeleteResult> RemoveConnectionAsync(Connection connection);
    Task<Group> GetMessageGroupAsync(string groupName);

    Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams);
    Task<IEnumerable<MessageDto>> GetMessageThread(string currentUserName, string recipientUserName);
}