using DatingApp.Shared.Queries;
using LikeApi.DTOs;
using LikeApi.Interfaces;
using LikeAPI.Queries;
using MediatR;
using System.Security.Claims;
using MassTransit;

namespace LikeAPI.QueryHandlers;

public class GetLikesQueryHandler : IRequestHandler<GetLikesQuery, List<LikeDto>>
{
    private readonly ILikesRepository _likesRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IScopedClientFactory _scopedClientFactory;

    public GetLikesQueryHandler(ILikesRepository likesRepository, IHttpContextAccessor httpContextAccessor, IScopedClientFactory scopedClientFactory)
    {
        _scopedClientFactory = scopedClientFactory;
        _httpContextAccessor = httpContextAccessor;
        _likesRepository = likesRepository;
    }

    public async Task<List<LikeDto>> Handle(GetLikesQuery request, CancellationToken cancellationToken)
    {
        request.LikesParams.Username = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name)?.Value!;
        var likes = await _likesRepository.GetUserLikes(request.LikesParams);
        
        var likesDto = new List<LikeDto>();
        foreach (var like in likes)
        {
            var userName = request.LikesParams.Predicate == "liked" ? like.TargetUserName : like.SourceUserName;

            var requestClient = _scopedClientFactory.CreateRequestClient<MemberQuery>();

            var memberQueryResponse = await requestClient.GetResponse<MemberQueryResponse>(new MemberQuery
            {
                MemberName = userName
            });

            var likedUser = memberQueryResponse.Message.Member;

            var likeDto = new LikeDto
            {
                Id = likedUser.Id,
                UserName = likedUser.UserName,
                PhotoUrl = likedUser.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                KnownAs = likedUser.KnownAs,
                Age = likedUser.Age,
                City = likedUser.City
            };

            likesDto.Add(likeDto);
        }

        return likesDto;
    }
}