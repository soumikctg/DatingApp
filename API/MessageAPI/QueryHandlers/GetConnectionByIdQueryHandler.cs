using MediatR;
using MessageAPI.Interfaces;
using MessageAPI.Models;
using MessageAPI.Queries;

namespace MessageAPI.QueryHandlers
{
    public class GetConnectionByIdQueryHandler : IRequestHandler<GetConnectionByIdQuery, Connection>
    {
        private readonly IMessageRepository _messageRepository;

        public GetConnectionByIdQueryHandler(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task<Connection> Handle(GetConnectionByIdQuery request, CancellationToken cancellationToken)
        {
            var connection = await _messageRepository.GetConnectionByIdAsync(request.ConnectionId);
            return connection;
        }
    }
}
