using MediatR;
using MessageAPI.Commands;
using MessageAPI.Interfaces;
using MessageAPI.Models;

namespace MessageAPI.CommandHandlers
{
    public class AddGroupCommandHandler : IRequestHandler<AddGroupCommand>
    {
        private readonly IMessageRepository _messageRepository;

        public AddGroupCommandHandler(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task Handle(AddGroupCommand request, CancellationToken cancellationToken)
        {
            await _messageRepository.AddGroupAsync(request.Group);
        }
    }
}
