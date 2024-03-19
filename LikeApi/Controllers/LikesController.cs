using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using LikeApi.DTOs;
using LikeApi.Extensions;
using LikeApi.Helpers;
using LikeApi.Interfaces;
using LikeApi.Models;

namespace LikeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikesController : ControllerBase
    {
        private readonly ILikesRepository _likesRepository;
        private readonly IUserApiService _userApiService;

        public LikesController(ILikesRepository likesRepository, IUserApiService userApiService)
        {
            _userApiService = userApiService;
            _likesRepository = likesRepository;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AddUserLike(Likes like)
        {
            try
            {
                await _likesRepository.AddLikeAsync(like);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("{username}")]
        public async Task<ActionResult> AddLike(string username)
        {
            var sourceUserName = User.FindFirst(ClaimTypes.Name)?.Value;

            var likedUser = await _userApiService.GetUserApiResponseAsync(username);
            /*var sourceUser = await _likesRepository.GetUserWithLikes(sourceUserId);*/

            if (likedUser == null) return NotFound();
            if (sourceUserName == username) return BadRequest("You cannot like yourself");

            var userLike = await _likesRepository.GetUserLike(sourceUserName, username);
            if (userLike != null) return BadRequest("You already liked this user");

            userLike = new Likes
            {
                Id = Guid.NewGuid().ToString(),
                SourceUserName = sourceUserName,
                TargetUserName = username
            };

            await _likesRepository.AddLikeAsync(userLike);
            return Ok("You liked " + likedUser.KnownAs);
        }


        [HttpGet]
        public async Task<ActionResult<PagedList<LikeDto>>> GetUserLikes([FromQuery] LikesParams likesParams)
        {
            likesParams.Username = User.FindFirst(ClaimTypes.Name)?.Value!;
            var likes = await _likesRepository.GetUserLikes(likesParams);
            var likesDto = new List<LikeDto>();
            foreach (var like in likes)
            {
                var userName = likesParams.Predicate == "liked" ? like.TargetUserName : like.SourceUserName;
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

            var pagedUsers =
                new PagedList<LikeDto>(likesDto, likesDto.Count, likesParams.PageNumber, likesParams.PageSize);

            Response.AddPaginationHeader(new PaginationHeader(pagedUsers.CurrentPage, pagedUsers.PageSize, pagedUsers.TotalCount, pagedUsers.TotalPages));
            return Ok(pagedUsers);
        }
    }
}
