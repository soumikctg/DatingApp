using MediatR;
using MessageAPI.Interfaces;
using MessageAPI.Models;
using MessageAPI.Queries;

namespace MessageAPI.QueryHandlers
{
    public class GetMessageGroupQueryHandler : IRequestHandler<GetMessageGroupQuery, Group>
    {
        private readonly IMessageRepository _messageRepository;

        public GetMessageGroupQueryHandler(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task<Group> Handle(GetMessageGroupQuery request, CancellationToken cancellationToken)
        {
            var group = await _messageRepository.GetMessageGroupAsync(request.GroupName);
            return group;
        }
    }
}
