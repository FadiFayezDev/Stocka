using Application.Bases;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Products;
using AutoMapper;
using Domain.Bases;
using Domain.Entities.Core;
using Domain.Entities.Products;
using Domain.Enums;
using Domain.Primitives;
using Domain.Repositories.Commands;
using MediatR;
using System;

namespace Application.Features.Commands.Warehouse.Create
{
    public class CreateWarehouseCommand : IRequest<Response<WarehouseDto>>
    {
        public string Name { get; set; } = null!;
        public int Type { get; set; }
        public string Location { get; set; } = null!;
        public string? Description { get; set; }
    }

    public class CreateWarehouseCommandHandler : BaseHandler<IWarehouseCommandRepository>, IRequestHandler<CreateWarehouseCommand, Response<WarehouseDto>>
    {
        private readonly IBranchCommandRepository _branchRepo;
        private readonly ICurrentUserContext _currentUser;

        public CreateWarehouseCommandHandler(
            IWarehouseCommandRepository repository, 
            IBranchCommandRepository branchRepo,
            IMapper mapper, 
            IUnitOfWork unitOfWork, 
            ICurrentUserContext userContext)
            : base(mapper, repository, unitOfWork)
        {
            _branchRepo = branchRepo;
            _currentUser = userContext;
        }

        public async Task<Response<WarehouseDto>> Handle(CreateWarehouseCommand request, CancellationToken cancellationToken)
        {
            var brandId = _currentUser.ActiveBrandId;
            var branchId = _currentUser.ActiveBranchId;

            var warehouse = new Domain.Entities.Products.Warehouse(
                new BrandId(brandId),
                request.Name,
                (WarehouseType)request.Type,
                request.Location,
                request.Description);

            if (branchId == null)
                return await ExecuteCreateAsync<Domain.Entities.Products.Warehouse, WarehouseDto>(
                    warehouse,
                    async (w) => await _repo.CreateAsync(w),
                    cancellationToken);
            else
            {
                var branch = await _branchRepo.GetByIdAsync(branchId.Value);
                if (branch == null)
                    throw new BusinessException("Branch not found");

                branch.AddWarehouse(warehouse);

                return await ExecuteCreateAsync<Domain.Entities.Products.Warehouse, WarehouseDto>(
                    warehouse,
                    async (w) => 
                    {
                        await _repo.CreateAsync(w);
                        await _branchRepo.UpdateAsync(branch);
                        return true;
                    },
                    cancellationToken);
            }
        }
    }
}
