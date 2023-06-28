﻿using Application.Command;
using Application.Common;
using Application.Common.Dto;
using Application.Query;
using Domain.Model;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("profile")]
        [ProducesDefaultResponseType(typeof(UserDto))]
        public async Task<IActionResult> EditProfile([FromBody] EditProfileCommand command)
        {
            var authorization = HttpContext.Request.Headers["Authorization"].ToString();
            command.Token = authorization;
            var response = await _mediator.Send(command);
            if (!response.Error)
                return Ok(response);
            else
            {
                var errorResponse = new BaseResponse<Exception>
                {
                    Exception = response.Exception,
                    Message = response.Message
                };
                return new ErrorHandling<Exception>(errorResponse);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Staff,Admin")]
        [HttpPut]
        [ProducesDefaultResponseType(typeof(UserDto))]
        public async Task<IActionResult> EditUser([FromBody] EditUserCommand command)
        {
            var response = await _mediator.Send(command);
            if (!response.Error)
                return Ok(response);
            else
            {
                var errorResponse = new BaseResponse<Exception>
                {
                    Exception = response.Exception,
                    Message = response.Message
                };
                return new ErrorHandling<Exception>(errorResponse);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Staff,Admin")]
        [HttpGet]
        [ProducesDefaultResponseType(typeof(List<UserDto>))]
        public async Task<IActionResult> GetUsers()
        {
            var query = new GetUsersQuery();
            var response = await _mediator.Send(query);
            if (!response.Error)
                return Ok(response);
            else
            {
                var errorResponse = new BaseResponse<Exception>
                {
                    Exception = response.Exception,
                    Message = response.Message
                };
                return new ErrorHandling<Exception>(errorResponse);
            }
        }
    }
}