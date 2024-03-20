using Microsoft.AspNetCore.Mvc;
using LikeAPI.Commands;
using LikeApi.DTOs;
using LikeApi.Extensions;
using LikeApi.Helpers;
using LikeAPI.Queries;
using MediatR;

namespace LikeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikesController : ControllerBase
    {

        private readonly IMediator _mediator;

        public LikesController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost("{username}")]
        public async Task<ActionResult> AddLike(string username)
        {
            var addLike = new AddLikeCommand
            {
                TargetUserName = username
            };
            try
            {
                await _mediator.Send(addLike);

                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return BadRequest(e.Message);
            }
        }


        [HttpGet]
        public async Task<ActionResult<PagedList<LikeDto>>> GetUserLikes([FromQuery] LikesParams likesParams)
        {

            var getLikes = new GetLikesQuery
            {
                LikesParams = likesParams
            };

            var likes = await _mediator.Send(getLikes);


            var pagedUsers =
                new PagedList<LikeDto>(likes, likes.Count, likesParams.PageNumber, likesParams.PageSize);

            Response.AddPaginationHeader(new PaginationHeader(pagedUsers.CurrentPage, pagedUsers.PageSize, pagedUsers.TotalCount, pagedUsers.TotalPages));
            return Ok(pagedUsers);
        }





        /*        [HttpPost("[action]")]
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
        }*/

    }
}
