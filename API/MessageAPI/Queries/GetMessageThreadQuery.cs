using MediatR;
using MessageAPI.DTOs;

namespace MessageAPI.Queries
{
    public class GetMessageThreadQuery:IRequest<IEnumerable<MessageDto>>
    {
        public string UserName { get; set; }
        public string CurrentUserName { get; set; }
    }
}
