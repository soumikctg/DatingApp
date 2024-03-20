﻿using UserAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using UserAPI.DTOs;
using UserAPI.Interfaces;

namespace UserAPI.Controllers
{
    public class AccountsController : BaseAPIController
    {
        private readonly IAccountsService _accountsService;


        public AccountsController(IAccountsService accountsService)
        {
            _accountsService = accountsService;
        }


        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            try
            {
                var user = await _accountsService.UserRegistration(registerDto);

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
                var user = await _accountsService.UserLogin(loginDto);
                return user;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return Unauthorized(e.Message);
            }
        }

    }
}