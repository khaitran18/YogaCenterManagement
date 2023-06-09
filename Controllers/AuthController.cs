﻿using Application.Command;
using Application.Common.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Common;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthCommand command)
        {
            var response = await _mediator.Send(command);
            if (!response.Error) return Ok(response);
            else
            {
                var ErrorResponse = new BaseResponse<Exception>
                {
                    Exception = response.Exception,
                    Message = response.Message
                };
                return new ErrorHandling<Exception>(ErrorResponse);
            }
        }
        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] SignUpCommand command)
        {
            var response = await _mediator.Send(command);
            if (!response.Error) return Ok(response);
            else
            {
                var ErrorResponse = new BaseResponse<Exception>
                {
                    Exception = response.Exception,
                    Message = response.Message
                };
                return new ErrorHandling<Exception>(ErrorResponse);
            }
        }

        [HttpPost("signup/{role}")]
        public async Task<IActionResult> SignupRole([FromBody] SignUpCommand command
            , [FromRoute] string role
            , [FromHeader] string Authorization)
        {
            command.Role = role;
            command.Token = Authorization;
            var response = await _mediator.Send(command);
            if (!response.Error) return Ok(response);
            else
            {
                var ErrorResponse = new BaseResponse<Exception> 
                { 
                    Exception = response.Exception,
                    Message = response.Message
                };
                return new ErrorHandling<Exception>(ErrorResponse);
            }
        }
        [HttpPost("verify")]
        public async Task<IActionResult> Verify([FromQuery] VerifyEmailCommand command)
        {
            var response = await _mediator.Send(command);
            if (!response.Error) return Ok(response);
            else
            {
                var ErrorResponse = new BaseResponse<Exception> 
                { 
                    Exception = response.Exception,
                    Message = response.Message
                };
                return new ErrorHandling<Exception>(ErrorResponse);
            }
        }
    }
}
