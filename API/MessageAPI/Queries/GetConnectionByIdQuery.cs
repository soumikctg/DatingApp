using MediatR;
using MessageAPI.Models;

namespace MessageAPI.Queries
{
    public class GetConnectionByIdQuery:IRequest<Connection>
    {
        public string ConnectionId { get; set;}
    }
}
