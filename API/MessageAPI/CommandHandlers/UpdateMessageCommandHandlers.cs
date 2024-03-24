using MediatR;
using MessageAPI.Commands;
using MessageAPI.Interfaces;

namespace MessageAPI.CommandHandlers
{
    public class UpdateMessageCommandHandlers : IRequestHandler<UpdateMessageCommand>
    {
        private readonly IMessageRepository _messageRepository;

        public UpdateMessageCommandHandlers(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task Handle(UpdateMessageCommand request, CancellationToken cancellationToken)
        {
            await _messageRepository.UpdateMessageAsync(request.Message);
        }
    }
}
