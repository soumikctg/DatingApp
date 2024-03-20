using System.Security.Claims;
using UserAPI.Entities;
using AutoMapper;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using User.Contracts.Dtos;
using UserAPI.Commands;
using UserAPI.DTOs;
using UserAPI.Extensions;
using UserAPI.Helpers;
using UserAPI.Interfaces;
using UserAPI.Queries;

namespace UserAPI.Controllers
{
    [Authorize]
    public class UsersController : BaseAPIController
    {
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        private readonly IUnitOfWork _uow;
        private readonly IUserRepository _userRepository;
        private readonly IMediator _mediator;

        public UsersController(IUnitOfWork uow, IMapper mapper, IPhotoService photoService,
            IUserRepository userRepository, IMediator mediator)
        {
            _mediator = mediator;
            _uow = uow;
            _photoService = photoService;
            _mapper = mapper;
            _userRepository = userRepository;
        }
        [HttpGet]
        public async Task<ActionResult<PagedList<MemberDto>>> GetUsers([FromQuery] UserParams userParams)
        {
            var getUsersQuery = new GetUsersQuery
            {
                UserParams = userParams
            };

            var users = await _mediator.Send(getUsersQuery);

            Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages));

            return Ok(users);
        }

        [HttpGet("{username}")] //api/users/username
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            var user = new GetUserByNameQuery
            {
                UserName = username
            };
            return await _mediator.Send(user);

        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {

            try
            {
                var updateUser = new UserUpdateCommand
                {
                    MemberUpdateDto = memberUpdateDto
                };

                await _mediator.Send(updateUser);
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e.Message);
            }
        }


        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {

            var addPhoto = new PhotoAddCommand
            {
                File = file
            };


            try
            {
                await _mediator.Send(addPhoto);
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e.Message);
            }

            /*var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var user = await _userRepository.GetUserByUserNameAsync(username);
            if (user == null) return NotFound();

            var result = await _photoService.AddPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if (user.Photos.Count == 0) photo.IsMain = true;

            user.Photos.Add(photo);

            if (await _uow.SaveChangesAsync() > 0)
            {
                return CreatedAtAction(nameof(GetUser),
                    new { username },
                    _mapper.Map<PhotoDto>(photo));
            }

            return BadRequest("Problem adding photo");*/
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var updatePhoto = new SetMainPhotoCommand
            {
                PhotoId = photoId
            };

            try
            {
                await _mediator.Send(updatePhoto);
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {

            var deletePhoto = new DeletePhotoCommand
            {
                PhotoId = photoId
            };

            try
            {
                await _mediator.Send(deletePhoto);
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


        }
    }
}
