using MediatR;
using MessageAPI.DTOs;
using MessageAPI.Helpers;

namespace MessageAPI.Queries
{
    public class GetMessageForUserQuery:IRequest<PagedList<MessageDto>>
    {
        public MessageParams MessageParams { get; set; }
    }
}
