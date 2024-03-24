using MediatR;
using MessageAPI.Models;

namespace MessageAPI.Queries
{
    public class GetConnectionQuery:IRequest<Connection>
    {
        public string UserName { get; set; }
    }
}
