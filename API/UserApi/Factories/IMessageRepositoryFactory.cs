using UserAPI.Interfaces;

namespace UserAPI.Factories;

public interface IMessageRepositoryFactory
{
    IMessageRepository GetMessageRepository();
}