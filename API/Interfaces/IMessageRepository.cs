using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IMessageRepository
    {
        void AddMessage(Message message);
        Task AddMessageAsync(NewMessage newMessage);
        void DeleteMessage(Message message);
        Task DeleteMessageAsync(string id);
        Task<Message> GetMessage(int id);
        Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams);
        Task<IEnumerable<MessageDto>> GetMessageThread(string currentUserName, string recipientUserName);

        void AddGroup(Group group);
        Task AddGroupAsync(NewGroup group);

        void RemoveConnection(Connection connection);
        Task AddConnectionAsync(NewConnection connection);
        Task<Connection> GetConnection(string connectionId);
        Task<NewConnection> GetConnectionAsync(string connectionId);
        Task<Group> GetMessageGroup(string groupName);
        Task<NewGroup> GetMessageGroupAsync(string groupName);
        Task<Group> GetGroupForConnection(string connectionId);
    }
}
