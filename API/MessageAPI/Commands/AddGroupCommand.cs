using MediatR;
using MessageAPI.Models;

namespace MessageAPI.Commands
{
    public class AddGroupCommand:IRequest
    {
        public Group Group { get; set; }
    }
}
