using MediatR;
using MessageAPI.Models;
using MongoDB.Driver;

namespace MessageAPI.Commands
{
    public class RemoveConnectionCommand:IRequest
    {
        public Connection Connection { get; set; }
    }
}
