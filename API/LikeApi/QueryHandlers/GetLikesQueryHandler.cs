using LikeApi.DTOs;
using LikeApi.Interfaces;
using LikeAPI.Queries;
using MediatR;
using System.Security.Claims;

namespace LikeAPI.QueryHandlers
{
    public class GetLikesQueryHandler : IRequestHandler<GetLikesQuery, List<LikeDto>>
    {
        private readonly ILikesRepository _likesRepository;
        private readonly IUserApiService _userApiService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetLikesQueryHandler(ILikesRepository likesRepository, IUserApiService userApiService, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _userApiService = userApiService;
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
                var likedUser = await _userApiService.GetUserApiResponseAsync(userName);

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
}
