using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Core;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCases.BranchCases
{
    public class PartialUpdateBranchCommand : IRequest<BranchDto>
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
    }

    public class PartialUpdateBranchCommandHandler : IRequestHandler<PartialUpdateBranchCommand, BranchDto>
    {
        private readonly IBranchCommandRepository _branchCommand;

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PartialUpdateBranchCommandHandler(IBranchCommandRepository branchCommand, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _branchCommand = branchCommand;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<BranchDto> Handle(PartialUpdateBranchCommand request, CancellationToken cancellationToken)
        {
            var existingEntity = await _branchCommand.GetByIdAsync(request.Id);

            if (existingEntity == null)
                throw new BusinessException("Branch is not found.");

            if (request.Name != null)
                existingEntity.UpdateName(request.Name);

            await _branchCommand.UpdateAsync(existingEntity);
            
            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (result < 0)
                throw new BusinessException("Branch is not saved");

            return _mapper.Map<BranchDto>(existingEntity);
        }
    }
}
