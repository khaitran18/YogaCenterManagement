using Application.Command;
using Application.Common.Dto;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Api;
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
        [ProducesDefaultResponseType(typeof(AuthResponseDto))]
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
    }
}
