using MediatR;
using MessageAPI.Commands;
using MessageAPI.Interfaces;

namespace MessageAPI.CommandHandlers
{
    public class AddMessageCommandHandler : IRequestHandler<AddMessageCommand>
    {
        private readonly IMessageRepository _messageRepository;

        public AddMessageCommandHandler(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task Handle(AddMessageCommand request, CancellationToken cancellationToken)
        {
            await _messageRepository.AddMessageAsync(request.Message);
        }
    }
}
