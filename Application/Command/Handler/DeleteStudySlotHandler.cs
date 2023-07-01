using Application.Common;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.Handler
{
    public class DeleteStudySlotHandler : IRequestHandler<DeleteStudySlotCommand, BaseResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteStudySlotHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }
        public async Task<BaseResponse<bool>> Handle(DeleteStudySlotCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<bool>();
            try
            {
                var resutl = await _unitOfWork.ScheduleRepository.DeleteStudySlot(request.StudySlotId);
                response.Result = resutl;
            }
            catch (Exception)
            {
                response.Error = true;
                throw;
            }
            return response;
        }
    }
}
