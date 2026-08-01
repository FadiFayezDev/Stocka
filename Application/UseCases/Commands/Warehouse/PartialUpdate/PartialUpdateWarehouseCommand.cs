using Application.Bases;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Products;
using AutoMapper;
using Domain.Enums;
using Domain.Repositories.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCases.Commands.Warehouse.PartialUpdate
{
    public class PartialUpdateWarehouseCommand : IRequest<Response<WarehouseDto>>
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public int? Type { get; set; }
        public string? Location { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
    }

    public class PartialUpdateWarehouseCommandHandler : BaseHandler<IWarehouseCommandRepository>, IRequestHandler<PartialUpdateWarehouseCommand, Response<WarehouseDto>>
    {
        public PartialUpdateWarehouseCommandHandler(IWarehouseCommandRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
            : base(mapper, repository, unitOfWork)
        {
        }
        public async Task<Response<WarehouseDto>> Handle(PartialUpdateWarehouseCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null)
                throw new BusinessException("Warehouse not found");

            if (request.Name != null)
                existing.UpdateName(request.Name);
            if (request.Type.HasValue)
                existing.ChangeType((WarehouseType)request.Type);
            if (request.Location != null)
                existing.UpdateLocation(request.Location);
            if (request.Description != null)
                existing.UpdateDescription(request.Description);
            if (request.IsActive.HasValue)
            {
                if (request.IsActive.Value)
                    existing.Activate();
                else
                    existing.Deactivate();
            }

            return await ExecuteUpdateAsync<Domain.Entities.Products.Warehouse, WarehouseDto>(
                existing,
                async (w) => await _repo.UpdateAsync(w),
                cancellationToken);
        }
    }
}