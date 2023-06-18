using Application.Command;
using Application.Common.Dto;
using Application.Query;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClassController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/<ClassController>
        [HttpGet("notification")]
        [ProducesDefaultResponseType(typeof(ClassNotificationDto))]
        public async Task<IActionResult> ClassNotification([FromQuery]ClassNotificationQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Lecturer,Staff,Admin")]
        [HttpPost("notification/{id}")]
        [ProducesDefaultResponseType(typeof(ClassNotificationDto))]
        public async Task<IActionResult> ClassNotification([FromHeader] string? Authorization,[FromRoute] int id,[FromBody]CreateNotificationCommand command)
        {
            try
            {
                command.token = Authorization;
                command.scheduleid = id;
                await _mediator.Send(command);
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
            
        }
    }
}
