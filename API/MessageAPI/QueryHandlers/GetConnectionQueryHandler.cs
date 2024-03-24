using MassTransit;
using MediatR;
using MessageAPI.Interfaces;
using MessageAPI.Models;
using MessageAPI.Queries;

namespace MessageAPI.QueryHandlers
{
    public class GetConnectionQueryHandler : IRequestHandler<GetConnectionQuery, Connection>
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IScopedClientFactory _scopedClientFactory;

        public GetConnectionQueryHandler(IMessageRepository messageRepository, IScopedClientFactory scopedClientFactory)
        {
            _scopedClientFactory = scopedClientFactory;
            _messageRepository = messageRepository;
        }

        public async Task<Connection> Handle(GetConnectionQuery request, CancellationToken cancellationToken)
        {
            var connection = await _messageRepository.GetConnectionAsync(request.UserName);
            return connection;
        }
    }
}
