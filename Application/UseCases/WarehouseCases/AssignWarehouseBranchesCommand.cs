using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Products;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.WarehouseCases
{
    public class AssignWarehouseBranchesCommand : IRequest<WarehouseDto>
    {
        public Guid WarehouseId { get; set; }
        public List<Guid> BranchIds { get; set; } = new();
    }

    public class AssignWarehouseBranchesCommandHandler : IRequestHandler<AssignWarehouseBranchesCommand, WarehouseDto>
    {
        private readonly IWarehouseCommandRepository _warehouseCommand;
        private readonly ICurrentUserContext _currentUser;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<WarehouseDto> _logger;

        public AssignWarehouseBranchesCommandHandler(
            IWarehouseCommandRepository warehouseCommand,
            ICurrentUserContext currentUser,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<WarehouseDto> logger)
        {
            _warehouseCommand = warehouseCommand;
            _currentUser = currentUser;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<WarehouseDto> Handle(AssignWarehouseBranchesCommand request, CancellationToken cancellationToken)
        {
            var brandId = _currentUser.ActiveBrandId;

            var warehouse = await _warehouseCommand.GetByIdAsync(request.WarehouseId);
            if (warehouse == null || warehouse.BrandId.Value != brandId)
                throw new NotFoundException("Warehouse not found.");

            await _warehouseCommand.ReplaceBranchLinksAsync(request.WarehouseId, request.BranchIds, brandId);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Warehouse {WarehouseId} branches updated", request.WarehouseId);

            return _mapper.Map<WarehouseDto>(warehouse);
        }
    }
}