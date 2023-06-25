using Application.Command;
using Application.Common;
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
        public async Task<IActionResult> ClassNotification([FromQuery] ClassNotificationQuery query)
        {
            var response = await _mediator.Send(query);
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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Lecturer,Staff,Admin")]
        [HttpPost("notification/{id}")]
        [ProducesDefaultResponseType(typeof(ClassNotificationDto))]
        public async Task<IActionResult> ClassNotification(
            [FromHeader] string? Authorization
            , [FromRoute] int id
            , [FromBody] CreateNotificationCommand command)
        {
            command.token = Authorization;
            command.scheduleid = id;
            var response = await _mediator.Send(command);
            if (!response.Error)
                return Ok(response);
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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[Authorize(Roles = "Staff,Admin,Lecturer")]
        [HttpPost("slot")]
        [ProducesDefaultResponseType(typeof(StudySlotDto))]
        public async Task<IActionResult> CreateStudySlot(
                [FromHeader] string? Authorization
                , [FromBody] CreateStudySlotCommand command)
        {
            command.token = Authorization;

            var response = await _mediator.Send(command);
            if (!response.Error)
                return Ok(response);
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

        [HttpPost("availabledate")]
        public async Task<IActionResult> AddAvailableDate(
        //[FromHeader] string? Authorization
        //, 
        [FromBody] AddAvailableDateCommand command)
        {
            //command.token = Authorization;

            var response = await _mediator.Send(command);
            if (!response.Error)
                return Ok(response);
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

        [HttpGet("availabledate/{slotId}")]
        public async Task<IActionResult> GetAvailableDateBySlotId([FromRoute] int slotId)
        {
            //command.token = Authorization;

            var response = await _mediator.Send(new AvailableDateQuery { SlotId = slotId});
            if (!response.Error)
                return Ok(response);
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
