using Application.Common;
using Application.Interfaces;
using AutoMapper;
using Domain.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.Handler
{
    public class UpdateStudySlotHandler : IRequestHandler<UpdateStudySlotCommand, BaseResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateStudySlotHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            
        }
        public async Task<BaseResponse<bool>> Handle(UpdateStudySlotCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<bool>();
            try
            {
                var studySlotModel = _mapper.Map<StudySlotModel>(request.StudySlot);
                var result = await  _unitOfWork.ScheduleRepository.UpdateStudySlot(studySlotModel);
                response.Result = result;
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
