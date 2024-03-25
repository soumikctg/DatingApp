using MediatR;
using MessageAPI.Models;

namespace MessageAPI.Queries
{
    public class GetMessageGroupQuery:IRequest<Group>
    {
        public string GroupName { get; set; }
    }
}
