using MediatR;
using MessageAPI.DTOs;

namespace MessageAPI.Commands
{
    public class UpdateMessageCommand:IRequest
    {
        public MessageDto Message { get; set; }
    }
}
