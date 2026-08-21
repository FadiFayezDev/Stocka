using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Core;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.BranchCases
{
    public class AssignBranchWarehousesCommand : IRequest<BranchDto>
    {
        public Guid BranchId { get; set; }
        public List<Guid> WarehouseIds { get; set; } = new();
    }

    public class AssignBranchWarehousesCommandHandler : IRequestHandler<AssignBranchWarehousesCommand, BranchDto>
    {
        private readonly IBranchCommandRepository _branchCommand;
        private readonly ICurrentUserContext _currentUser;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<BranchDto> _logger;

        public AssignBranchWarehousesCommandHandler(
            IBranchCommandRepository branchCommand,
            ICurrentUserContext currentUser,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<BranchDto> logger)
        {
            _branchCommand = branchCommand;
            _currentUser = currentUser;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BranchDto> Handle(AssignBranchWarehousesCommand request, CancellationToken cancellationToken)
        {
            var brandId = _currentUser.ActiveBrandId;

            var branch = await _branchCommand.GetByIdAsync(request.BranchId);
            if (branch == null || branch.BrandId.Value != brandId)
                throw new NotFoundException("Branch not found.");

            var requested = request.WarehouseIds.Distinct().ToList();
            var requestedSet = requested.ToHashSet();

            var toRemove = branch.WarehouseBranches
                .Where(wb => !requestedSet.Contains(wb.WarehouseId.Value))
                .Select(wb => wb.WarehouseId)
                .ToList();

            var existingIds = branch.WarehouseBranches.Select(wb => wb.WarehouseId.Value).ToHashSet();
            var toAdd = requested.Where(id => !existingIds.Contains(id)).ToList();

            foreach (var warehouseId in toRemove)
                branch.RemoveWarehouse(warehouseId);

            foreach (var warehouseId in toAdd)
                branch.AddWarehouse(new Domain.Primitives.WarehouseId(warehouseId));

            await _branchCommand.UpdateAsync(branch);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Branch {BranchId} warehouses updated", request.BranchId);

            return _mapper.Map<BranchDto>(branch);
        }
    }
}