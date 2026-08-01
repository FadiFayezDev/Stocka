using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Products;
using AutoMapper;
using Domain.Entities.Products;
using Domain.Enums;
using Domain.Primitives;
using Domain.Repositories.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.WarehouseCases
{
    public class CreateWarehouseCommand : IRequest<WarehouseDto>
    {
        public string Name { get; set; } = null!;
        public int Type { get; set; }
        public string Location { get; set; } = null!;
        public string? Description { get; set; }
    }

    public class CreateWarehouseCommandHandler : IRequestHandler<CreateWarehouseCommand, WarehouseDto>
    {
        private readonly ICurrentUserContext _currentUserContext;
        private readonly IWarehouseCommandRepository _warehouseCommand;
        private readonly IBranchCommandRepository _branchRepo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<WarehouseDto> _logger;

        public CreateWarehouseCommandHandler(
            ICurrentUserContext currentUserContext,
            IWarehouseCommandRepository warehouseCommand,
            IBranchCommandRepository branchRepo,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            ILogger<WarehouseDto> logger)
        {
            _currentUserContext = currentUserContext;
            _warehouseCommand = warehouseCommand;
            _branchRepo = branchRepo;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<WarehouseDto> Handle(CreateWarehouseCommand request, CancellationToken cancellationToken)
        {
            var brandId = _currentUserContext.ActiveBrandId;
            var branchId = _currentUserContext.ActiveBranchId;

            try
            {
                var warehouse = new Warehouse(new BrandId(brandId), request.Name, (WarehouseType)request.Type, request.Location, request.Description);

                if (branchId == null)
                {
                    await _warehouseCommand.CreateAsync(warehouse);
                }
                else
                {
                    var branch = await _branchRepo.GetByIdAsync(branchId.Value);
                    if (branch == null)
                        throw new BusinessException("Branch not found");

                    branch.AddWarehouse(warehouse);
                    await _warehouseCommand.CreateAsync(warehouse);
                    await _branchRepo.UpdateAsync(branch);
                }

                var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Warehouse is created");

                var dto = _mapper.Map<WarehouseDto>(warehouse);
                return dto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Warehouse creation failed");
                throw;
            }
        }
    }
}
