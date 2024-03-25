using MediatR;
using MessageAPI.Commands;
using MessageAPI.Interfaces;
using MongoDB.Driver;

namespace MessageAPI.CommandHandlers
{
    public class RemoveConnectionCommandHandler : IRequestHandler<RemoveConnectionCommand>
    {
        private readonly IMessageRepository _messageRepository;

        public RemoveConnectionCommandHandler(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task Handle(RemoveConnectionCommand request, CancellationToken cancellationToken)
        {
            await _messageRepository.RemoveConnectionAsync(request.Connection);
        }
    }
}
