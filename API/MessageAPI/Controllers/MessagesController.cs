using System.Security.Claims;
using DatingApp.Shared.Helpers;
using MassTransit;
using MediatR;
using MessageAPI.DTOs;
using MessageAPI.Extensions;
using MessageAPI.Helpers;
using MessageAPI.Interfaces;
using MessageAPI.Queries;
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
    private readonly IMediator _mediator;

    public MessagesController( IMessageRepository messageRepository, IScopedClientFactory scopedClientFactory,IMediator mediator)
    {
        _mediator = mediator;
        _scopedClientFactory = scopedClientFactory;
        _messageRepository = messageRepository;
    }

/*    [HttpPost("add-message")]
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
    }*/
    
    [HttpGet]
    public async Task<ActionResult<PagedList<MessageDto>>> GetMessagesForUser(
        [FromQuery] MessageParams messageParams)
    {
        messageParams.Username = User.FindFirst(ClaimTypes.Name)?.Value;

        if (!ValidationHelper.TryValidate(messageParams, out var validationResults))
        {
            return BadRequest(validationResults);
        }

        var messageQuery = new GetMessageForUserQuery
        {
            MessageParams = messageParams
        };

        var messages = await _mediator.Send(messageQuery);

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

        var messageThread = new GetMessageThreadQuery
        {
            UserName = username,
            CurrentUserName = currentUsername
        };

        var messages = await _mediator.Send(messageThread);

        return Ok(messages);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteMessage(string id)
    {
        var username = User.FindFirst(ClaimTypes.Name)?.Value;

        await _messageRepository.DeleteMessageAsync(id);

        return BadRequest("Problem deleting the message");
    }

    //public override bool TryValidateModel(object model)
    //{
    //    if (!ValidationHelper.TryValidate(model, out var validationResults))
    //    {
    //        throw new Exception(JsonSerializer.Serialize(validationResults));
    //    }

    //    return true;
    //}
}