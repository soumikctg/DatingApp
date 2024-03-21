using UserAPI.Interfaces;

namespace UserAPI.Factories;
// convention -> repository implementation name should be start with corresponding database provider name 

public class MessageRepositoryFactory : IMessageRepositoryFactory
{
    private readonly IConfiguration _configuration;
    private readonly IEnumerable<IMessageRepository> _messageRepositories;
    public MessageRepositoryFactory(IConfiguration configuration, IEnumerable<IMessageRepository> messageRepositories)
    {
        _messageRepositories = messageRepositories;
        _configuration = configuration;
    }

    public IMessageRepository GetMessageRepository()
    {
        var provider = _configuration["DatabaseConfig:Provider"];

        var instance =
            _messageRepositories.FirstOrDefault(x => x.GetType().Name.StartsWith(provider!));

        if (instance == null)
        {
            return _messageRepositories.First();
        }

        return instance;
    }
}