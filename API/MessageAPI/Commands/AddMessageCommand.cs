using MediatR;
using MessageAPI.Models;

namespace MessageAPI.Commands
{
    public class AddMessageCommand:IRequest
    {
        public Message Message { get; set; }
    }
}
