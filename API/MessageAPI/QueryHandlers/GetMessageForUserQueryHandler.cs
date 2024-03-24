using MediatR;
using MessageAPI.DTOs;
using MessageAPI.Helpers;
using MessageAPI.Interfaces;
using MessageAPI.Queries;

namespace MessageAPI.QueryHandlers
{
    public class GetMessageForUserQueryHandler : IRequestHandler<GetMessageForUserQuery, PagedList<MessageDto>>
    {
        private readonly IMessageRepository _messageRepository;

        public GetMessageForUserQueryHandler(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task<PagedList<MessageDto>> Handle(GetMessageForUserQuery request, CancellationToken cancellationToken)
        {
            var messages = await _messageRepository.GetMessagesForUser(request.MessageParams);
            return messages;
        }
    }
}
