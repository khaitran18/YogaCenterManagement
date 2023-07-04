using Application.Common;
using Application.Common.Dto;
using Application.Interfaces;
using Application.Service;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.Handler
{
    public class CreateChangeRequestClassHandler : IRequestHandler<CreateChangeRequestCommand, BaseResponse<ClassDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenServices;
        private readonly IMapper _mapper;

        public CreateChangeRequestClassHandler(IUnitOfWork unitOfWork, ITokenService tokenServices, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _tokenServices = tokenServices;
            _mapper = mapper;
        }
        public async Task<BaseResponse<ClassDto>> Handle(CreateChangeRequestCommand request, CancellationToken cancellationToken)
        {
            var resposne = new BaseResponse<ClassDto>();
            try
            {
                if (await _unitOfWork.ClassRepository.ExistChangeClassRequest(request.StudentId, request.FromClassId, request.ToClassId))
                {
                    resposne.Error = true;
                    resposne.Message = "Same request already exist";
                }
                else
                {
                    var classModel = await _unitOfWork.ClassRepository.RequestChangeClass(request.FromClassId, request.StudentId, request.ToClassId, request.Content);
                    var classDto = _mapper.Map<ClassDto>(classModel);
                    resposne.Result = classDto;
                }

            }
            catch (Exception)
            {
                resposne.Error = true;
                throw;
            }
            return resposne;
        }
    }
}
