using Application.Command;
using Application.Common.Dto;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Api;

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
                var i = new ErrorHandling(response.Exception);
                return i;
            }
        }
        //[HttpPost("signup")]
        //public async Task<IActionResult> Signup([FromBody] SignUpCommand command)
        //{
        //    return Ok(await _mediator.Send(command));
        //}
    }
}
