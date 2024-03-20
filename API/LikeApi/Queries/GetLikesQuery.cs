using LikeApi.DTOs;
using LikeApi.Helpers;
using MediatR;

namespace LikeAPI.Queries
{
    public class GetLikesQuery:IRequest<List<LikeDto>>
    {
        public LikesParams LikesParams { get; set; }
    }
}
