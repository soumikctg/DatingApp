using LikeAPI.Commands;
using LikeApi.Interfaces;
using LikeApi.Models;
using MediatR;
using System.Security.Claims;
using DatingApp.Shared.Queries;
using MassTransit;

namespace LikeAPI.CommandHandlers;

public class AddLikeCommandHandler : IRequestHandler<AddLikeCommand>
{
    private readonly ILikesRepository _likesRepository;
    private readonly IUserApiService _userApiService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IScopedClientFactory _scopedClientFactory;
    private readonly IRequestClient<MemberQuery> _requestClient;
    public AddLikeCommandHandler(
        ILikesRepository likesRepository, 
        IUserApiService userApiService, 
        IHttpContextAccessor httpContextAccessor, 
        IScopedClientFactory scopeClientFactory,
        IRequestClient<MemberQuery> requestClient)
    {
        _userApiService = userApiService;
        _likesRepository = likesRepository;
        _httpContextAccessor = httpContextAccessor;
        _scopedClientFactory = scopeClientFactory;
        _requestClient = requestClient;

    }

    public async Task Handle(AddLikeCommand request, CancellationToken cancellationToken)
    {
        var requestClient = _scopedClientFactory.CreateRequestClient<MemberQuery>();

        var memberQueryResponse = await requestClient.GetResponse<MemberQueryResponse>(new MemberQuery
        {
            MemberName = request.TargetUserName
        });

        var sourceUserName = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;

        // var likedUser = await _userApiService.GetUserApiResponseAsync(request.TargetUserName);
        var likedUser = memberQueryResponse.Message.Member;

        if (likedUser == null) throw new Exception("User not found");

        if (sourceUserName == request.TargetUserName) throw new Exception("You cannot like yourself!");

        var like = await _likesRepository.GetUserLike(sourceUserName, request.TargetUserName);

        if (like != null) throw new Exception("You already liked this user");

        var userLike = new Likes
        {
            Id = Guid.NewGuid().ToString(),
            SourceUserName = sourceUserName,
            TargetUserName = request.TargetUserName
        };

        await _likesRepository.AddLikeAsync(userLike);
    }
}