using System.Security.Claims;
using UserAPI.Data;
using UserAPI.Extensions;
using UserAPI.Factories;
using UserAPI.Helpers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UserAPI.DTOs;
using UserAPI.Entities;
using UserAPI.Interfaces;

namespace UserAPI.Controllers
{
    public class MessagesController : BaseAPIController
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly IUserRepository _userRepository;
        private readonly IMessageRepository _messageRepository;

        public MessagesController(IMapper mapper, IUnitOfWork uow, IUserRepository userRepository, IMessageRepository messageRepository)
        {
            _uow = uow;
            _mapper = mapper;
            _userRepository = userRepository;
            _messageRepository = messageRepository;
        }

        [HttpPost("add-message")]
        public async Task<IActionResult> AddMessage(NewMessage message)
        {
            try
            {
                await _messageRepository.AddMessageAsync(message);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal server error"+ e);
            }
        }

        [HttpDelete("delete-message")]
        public async Task<IActionResult> DeleteMessageFromDb(string id)
        {
            try
            {
                await _messageRepository.DeleteMessageAsync(id);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal server error" + e);
            }
        }

        [HttpPost]
        public async Task<ActionResult> SendMessageAsync(CreateMessageDto createMessageDto)
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;

            if (username == createMessageDto.RecipientUserName.ToLower())
                return BadRequest("You cannot send messages to yourself");

            var sender = await _userRepository.GetUserByUserNameAsync(username);
            var recipient = await _userRepository.GetUserByUserNameAsync(createMessageDto.RecipientUserName);

            if (recipient == null) return NotFound();

            var message = new NewMessage
            {
                SenderUserName = sender.UserName,
                SenderPhotoUrl = sender.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                RecipientUserName = recipient.UserName,
                RecipientPhotoUrl = recipient.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                Content = createMessageDto.Content

            };

            await _messageRepository.AddMessageAsync(message);

            return Ok();
        }
        /*
        [HttpGet]
        public async Task<ActionResult<PagedList<MessageDto>>> GetMessagesForUser(
            [FromQuery] MessageParams messageParams)
        {
            messageParams.Username = User.FindFirst(ClaimTypes.Name)?.Value;

            var messages = await _messageRepository.GetMessagesForUser(messageParams);

            Response.AddPaginationHeader(new PaginationHeader(
                messages.CurrentPage,
                messages.PageSize,
                messages.TotalCount,
                messages.TotalPages
                ));
            return messages;

        }

        [HttpGet("thread/{username}")]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageThread(string username)
        {
            var currentUsername = User.FindFirst(ClaimTypes.Name)?.Value;
            return Ok(await _uow.MessageRepository.GetMessageThread(currentUsername, username));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id)
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;

            var message = await _messageRepository.GetMessage(id);

            if (message.SenderUserName != username && message.RecipientUserName != username)
                return Unauthorized();

            if (message.SenderUserName == username) message.SenderDeleted = true;

            if (message.RecipientUserName == username) message.RecipientDeleted = true;

            if (message.SenderDeleted && message.RecipientDeleted)
            {
                _messageRepository.DeleteMessage(message);
            }

            if (await _uow.SaveChangesAsync() > 0) return Ok();

            return BadRequest("Problem deleting the message");
        }*/
    }
}
