using Application.Common.Dto;
using Application.Query;
using MediatR;
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
    }
}
