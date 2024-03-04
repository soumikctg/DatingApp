using API.Interfaces;

namespace API.Factories;

public interface IMessageRepositoryFactory
{
    IMessageRepository GetMessageRepository();
}