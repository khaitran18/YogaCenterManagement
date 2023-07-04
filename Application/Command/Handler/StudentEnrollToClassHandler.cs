using Application.Common;
using Application.Common.Dto;
using Application.Common.Exceptions;
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
    public class StudentEnrollToClassHandler : IRequestHandler<StudentEnrollToClassCommand, BaseResponse<PaymentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StudentEnrollToClassHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<BaseResponse<PaymentDto>> Handle(StudentEnrollToClassCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<PaymentDto>();
            try
            {
                var @class = await _unitOfWork.ClassRepository.GetClassById(request.PaymentDto.ClassId!.Value);
                if (@class.ClassStatus != 1)
                {
                    response.Error = true;
                    response.Exception = new BadRequestException("Class is not able to enroll");
                }
                else
                {
                    var paymentModel = _mapper.Map<PaymentModel>(request.PaymentDto);

                    var result = await _unitOfWork.ClassRepository.StudentEnrollToClass(paymentModel);
                    response.Result = _mapper.Map<PaymentDto>(result);
                }

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
