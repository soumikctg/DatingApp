using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserAPI.Commands;
using UserAPI.DTOs;

namespace UserAPI.Controllers;

public class AccountsController : BaseAPIController
{
    private readonly IMediator _mediator;


    public AccountsController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        try
        {
            var registerUserCommand = new RegisterUserCommand
            {
                RegisterDto = registerDto
            };

            var user = await _mediator.Send(registerUserCommand);

            return user;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message); // logger 
            return BadRequest(e.Message);
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        try
        {
            var loginUserCommand = new LoginUserCommand
            {
                LoginDto = loginDto
            };
            var user = await _mediator.Send(loginUserCommand);
            return user;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);

            return Unauthorized(e.Message);
        }
    }

}