using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserAPI.Helpers;
using UserAPI.Interfaces;
using UserAPI.Extensions;
using UserAPI.DTOs;
using UserAPI.Entities;

namespace UserAPI.Controllers
{

    public class LikesController : BaseAPIController
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserRepository _userRepository;
        private readonly ILikesRepository _likesRepository;

        public LikesController(IUnitOfWork uow, IUserRepository userRepository, ILikesRepository likesRepository)
        {
            _uow = uow;
            _userRepository = userRepository;
            _likesRepository = likesRepository;
        }

        [HttpPost("add-like")]
        public async Task<IActionResult> AddUserLike(Likes like)
        {
            try
            {
                await _likesRepository.AddLikeAsync(like);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal server error" + e);
            }
        }

        [HttpPost("{username}")]
        public async Task<ActionResult> AddLike(string username)
        {
            var sourceUserName = User.FindFirst(ClaimTypes.Name)?.Value;

            var likedUser = await _userRepository.GetUserByUserNameAsync(username);
            /*var sourceUser = await _likesRepository.GetUserWithLikes(sourceUserId);*/

            if (likedUser == null) return NotFound();
            if (sourceUserName == username) return BadRequest("You cannot like yourself");

            var userLike = await _likesRepository.GetUserLike(sourceUserName, username);
            if (userLike != null) return BadRequest("You already liked this user");

            userLike = new Likes
            {
                SourceUserName = sourceUserName,
                TargetUserName = username
            };

            await _likesRepository.AddLikeAsync(userLike);
            return Ok();
        }


        [HttpGet]
        public async Task<ActionResult<PagedList<LikeDto>>> GetUserLikes([FromQuery] LikesParams likesParams)
        {
            likesParams.UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var users = await _likesRepository.GetUserLikes(likesParams);

            Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages));
            return Ok(users);
        }

    }
}
