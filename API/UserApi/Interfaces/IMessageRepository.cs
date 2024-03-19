using MongoDB.Driver;
using UserAPI.DTOs;
using UserAPI.Entities;
using UserAPI.Helpers;

namespace UserAPI.Interfaces
{
    public interface IMessageRepository
    {
        void AddMessage(Message message);
        Task AddMessageAsync(NewMessage newMessage);
        Task UpdateMessageAsync(MessageDto newMessage);
        void DeleteMessage(Message message);
        Task DeleteMessageAsync(string id);
        Task<Message> GetMessage(int id);
        Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams);
        Task<IEnumerable<MessageDto>> GetMessageThread(string currentUserName, string recipientUserName);

        void AddGroup(Group group);
        Task AddGroupAsync(NewGroup group);

        void RemoveConnection(Connection connection);
        Task<DeleteResult> RemoveConnectionAsync(NewConnection connection);
        Task AddConnectionAsync(NewConnection connection);
        Task<Connection> GetConnection(string connectionId);
        Task<NewConnection> GetConnectionAsync(string connectionId);
        Task<NewConnection> GetConnectionByIdAsync(string Id);
        Task<Group> GetMessageGroup(string groupName);
        Task<NewGroup> GetMessageGroupAsync(string groupName);
        Task<Group> GetGroupForConnection(string connectionId);
    }
}
