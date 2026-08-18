using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Products;
using AutoMapper;
using Domain.Enums;
using Domain.Repositories.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.WarehouseCases
{
    public class UpdateWarehouseInformationCommand : IRequest<WarehouseDto>
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public int? Type { get; set; }
        public string? Location { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
    }

    public class UpdateWarehouseInformationCommandHandler : IRequestHandler<UpdateWarehouseInformationCommand, WarehouseDto>
    {
        private readonly IWarehouseCommandRepository _warehouseCommand;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<WarehouseDto> _logger;

        public UpdateWarehouseInformationCommandHandler(
            IWarehouseCommandRepository warehouseCommand,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            ILogger<WarehouseDto> logger)
        {
            _warehouseCommand = warehouseCommand;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<WarehouseDto> Handle(UpdateWarehouseInformationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existing = await _warehouseCommand.GetByIdAsync(request.Id);
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

                await _warehouseCommand.UpdateAsync(existing);
                var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Warehouse is partially updated");

                var dto = _mapper.Map<WarehouseDto>(existing);
                return dto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Warehouse partial update failed");
                throw;
            }
        }
    }
}
