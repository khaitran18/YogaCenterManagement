using Application.Command;
using Application.Common;
using Application.Common.Dto;
using Application.Query;
using Domain.Model;
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

        [HttpGet("{id}")]
        [ProducesDefaultResponseType(typeof(ClassDto))]
        public async Task<IActionResult> GetClassById([FromRoute] int id)
        {
            var response = await _mediator.Send(new GetClassByIdQuery { ClassId = id });
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
        [Authorize(Roles = "Staff,Admin,Lecturer")]
        [HttpPost]
        [ProducesDefaultResponseType(typeof(ClassDto))]
        public async Task<IActionResult> CreateClass(
                [FromHeader] string? Authorization
                , [FromForm] CreateClassCommand command)
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

        [HttpGet]
        [ProducesDefaultResponseType(typeof(PaginatedResult<ClassDto>))]
        public async Task<IActionResult> GetClasses([FromQuery] GetClassesQuery query)
        {
            var authorization = HttpContext.Request.Headers["Authorization"].ToString();
            query.Token = authorization;
            var response = await _mediator.Send(query);
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

        [HttpGet("schedule")]
        public async Task<IActionResult> GetSchedule([FromQuery] GetScheduleQuery query,
            [FromHeader] string? Authorization)
        {
            query.token = Authorization;
            var response = await _mediator.Send(query);
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
        [Authorize(Roles = "Staff,Admin,Lecturer")]
        [HttpPost("assignLecturer")]
        [ProducesDefaultResponseType(typeof(ClassDto))]
        public async Task<IActionResult> AssignLecturer(
                [FromHeader] string? Authorization
                , [FromBody] AssignLecturerCommand command)
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
        [HttpPost("notification")]
        [ProducesDefaultResponseType(typeof(ClassNotificationDto))]
        public async Task<IActionResult> ClassNotification(
            [FromHeader] string? Authorization
            , [FromBody] CreateNotificationCommand command)
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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[Authorize(Roles = "Staff,Admin,Lecturer")]
        [HttpGet("slot")]
        [ProducesDefaultResponseType(typeof(StudySlotDto))]
        public async Task<IActionResult> GetAllStudySlots(
        [FromHeader] string? Authorization
        )
        {
            
            var response = await _mediator.Send(new GetStudySlotsQuery());
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
        [HttpPut]
        [ProducesDefaultResponseType(typeof(ClassDto))]
        public async Task<IActionResult> EditClass(
                [FromHeader] string? Authorization
                , [FromForm] EditClassCommand command)
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

        [HttpDelete("slot/{slotId}")]
        public async Task<IActionResult> DeleteStudySlot([FromRoute] int slotId)
        {
            //command.token = Authorization;

            var response = await _mediator.Send(new DeleteStudySlotCommand { StudySlotId = slotId });
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

        [HttpPut("slot")]
        public async Task<IActionResult> UpdateStudySlot([FromBody] UpdateStudySlotCommand command)
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[Authorize(Roles = "Lecturer")]
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

        [HttpGet("availabledate/byslotid/{slotId}")]
        public async Task<IActionResult> GetAvailableDateBySlotId([FromRoute] int slotId)
        {
            //command.token = Authorization;

            var response = await _mediator.Send(new AvailableDateQuery { SlotId = slotId });
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
        [Authorize(Roles = "User")]
        [HttpGet("studyclass/{studentId}")]
        public async Task<IActionResult> GetStudyingClass([FromRoute] int studentId, [FromQuery] int page, [FromQuery] int pageSize)
        {
            //command.token = Authorization;

            var response = await _mediator.Send(new GetStudyingClassQuery { StudentId = studentId , Page = page, PageSize = pageSize});
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
        [Authorize(Roles = "User")]
        [HttpGet("studyclass")]
        public async Task<IActionResult> GetStudyingClass([FromQuery] GetStudyingClassByClassIdQuery query)
        {
            //command.token = Authorization;

            var response = await _mediator.Send(query);
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
        //[Authorize(Roles = "Lecturer")]
        [HttpGet("availabledate/bylecturerid/{lecturerId}")]
        public async Task<IActionResult> GetAvailableDateByLecturerId([FromRoute] int lecturerId)
        {
            //command.token = Authorization;

            var response = await _mediator.Send(new GetAvailableDateByLecturerId { LecturerId = lecturerId });
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
        [Authorize(Roles = "User")]
        [HttpPost("changeclass")]
        public async Task<IActionResult> CreateChangeClassRequest([FromBody] CreateChangeRequestCommand command)
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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Staff, Admin")]
        [HttpGet("changeclass")]
        public async Task<IActionResult> GetChangeClassRequests()
        {
            //command.token = Authorization;

            var response = await _mediator.Send(new GetChangeClassRequestsQuery());
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
        [Authorize(Roles = "User")]
        [HttpGet("changeclasses/{fromClassId}")]
        public async Task<IActionResult> GetChangeClass([FromRoute] int fromClassId)
        {
            //command.token = Authorization;

            var response = await _mediator.Send(new GetChangeClassQuery { FromClassId = fromClassId });
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
        [Authorize(Roles = "Staff, Admin")]
        [HttpPut("changeclass")]
        public async Task<IActionResult> UpdateApprovalStatus([FromBody] UpdateApprovalStatusCommand command)
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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "User")]
        [HttpPost("enroll")]
        public async Task<IActionResult> EnrollStudentToClass([FromBody] StudentEnrollToClassCommand command, [FromHeader] string? Authorization)
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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[Authorize(Roles = "Lecturer")]
        [HttpGet("teachclass/{lecturerId}")]
        public async Task<IActionResult> GetTeachingClass([FromRoute] int lecturerId, [FromQuery] int page, [FromQuery] int pageSize)
        {
            //command.token = Authorization;

            var response = await _mediator.Send(new GetTeachingClassQuery { LecturerId = lecturerId, Page = page, PageSize = pageSize });
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
        [Authorize(Roles = "Lecturer")]
        [HttpGet("teachclass")]
        public async Task<IActionResult> GetTeachingClassByClassID([FromQuery] GetTeachingClassByClassIdQuery query)
        {
            //command.token = Authorization;

            var response = await _mediator.Send(query);
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

        [HttpGet("availabledate")]
        public async Task<IActionResult> GetAllAvailableDates()
        {
            //command.token = Authorization;

            var response = await _mediator.Send(new GetAllAvailableDatesQuery());
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
        //[Authorize(Roles = "Lecturer")]
        [HttpDelete("availabledate")]
        public async Task<IActionResult> RemoveLecturerAvailableDate([FromQuery] RemoveAvailableDateCommand command)
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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "User")]
        [HttpGet("studiedclass/{studentId}")]
        public async Task<IActionResult> GetStudiedClass([FromRoute] int studentId, [FromQuery] int page, [FromQuery] int pageSize)
        {
            //command.token = Authorization;

            var response = await _mediator.Send(new GetStudiedClassQuery { StudentId = studentId, Page = page, PageSize = pageSize });
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
        //[Authorize(Roles = "User")]
        [HttpGet("taughtclass/{lecturerId}")]
        public async Task<IActionResult> GetTaughtClass([FromRoute] int lecturerId, [FromQuery] int page, [FromQuery] int pageSize)
        {
            //command.token = Authorization;

            var response = await _mediator.Send(new GetTaughtClassQuery { LecturerId = lecturerId, Page = page, PageSize = pageSize });
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
