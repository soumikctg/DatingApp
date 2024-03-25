using MediatR;
using MessageAPI.Models;

namespace MessageAPI.Commands
{
    public class AddConnectionCommand:IRequest
    {
        public Connection Connection { get; set; }
    }
}
