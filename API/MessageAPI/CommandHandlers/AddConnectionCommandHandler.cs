using MediatR;
using MessageAPI.Commands;
using MessageAPI.Interfaces;

namespace MessageAPI.CommandHandlers
{
    public class AddConnectionCommandHandler : IRequestHandler<AddConnectionCommand>
    {
        private readonly IMessageRepository _messageRepository;

        public AddConnectionCommandHandler(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task Handle(AddConnectionCommand request, CancellationToken cancellationToken)
        {
            await _messageRepository.AddConnectionAsync(request.Connection);
        }
    }
}
