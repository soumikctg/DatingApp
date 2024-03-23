using System.Security.Claims;
using AutoMapper;
using DatingApp.Shared.Queries;
using MassTransit;
using MessageAPI.DTOs;
using MessageAPI.Interfaces;
using MessageAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace MessageAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]

public class MessagesController:ControllerBase
{
    private readonly IMessageRepository _messageRepository;
    private readonly IScopedClientFactory _scopedClientFactory;

    public MessagesController( IMessageRepository messageRepository, IScopedClientFactory scopedClientFactory)
    {
        _scopedClientFactory = scopedClientFactory;
        _messageRepository = messageRepository;
    }

    [HttpPost("add-message")]
    public async Task<IActionResult> AddMessage(Message message)
    {
        try
        {
            await _messageRepository.AddMessageAsync(message);
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal server error" + e);
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

        var requestClient = _scopedClientFactory.CreateRequestClient<MemberQuery>();

        var MemberQueryResponseSender = await requestClient.GetResponse<MemberQueryResponse>(new MemberQuery
        {
            MemberName = username
        });

        var MemberQueryResponseRecipient = await requestClient.GetResponse<MemberQueryResponse>(new MemberQuery
        {
            MemberName = createMessageDto.RecipientUserName
        });

        var sender = MemberQueryResponseSender.Message.Member;
        var recipient = MemberQueryResponseRecipient.Message.Member;

        if (recipient == null) return NotFound();

        var message = new Message
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