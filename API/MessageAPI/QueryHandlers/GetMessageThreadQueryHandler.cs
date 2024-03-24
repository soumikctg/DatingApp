using MediatR;
using MessageAPI.DTOs;
using MessageAPI.Interfaces;
using MessageAPI.Queries;

namespace MessageAPI.QueryHandlers
{
    public class GetMessageThreadQueryHandler : IRequestHandler<GetMessageThreadQuery, IEnumerable<MessageDto>>
    {
        private readonly IMessageRepository _messageRepository;

        public GetMessageThreadQueryHandler(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task<IEnumerable<MessageDto>> Handle(GetMessageThreadQuery request, CancellationToken cancellationToken)
        {
            var messages = await _messageRepository.GetMessageThread(request.CurrentUserName, request.UserName);
            return messages;
        }
    }
}
