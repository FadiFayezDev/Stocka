using Application.Bases;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Products;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Commands.Warehouse.Update
{
    public class UpdateWarehouseCommand : IRequest<Response<WarehouseDto>>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public int Type { get; set; }
        public string Location { get; set; } = null!;
        public string? Description { get; set; }
    }

    public class UpdateWarehouseCommandHandler : BaseHandler<IWarehouseCommandRepository>, IRequestHandler<UpdateWarehouseCommand, Response<WarehouseDto>>
    {
        public UpdateWarehouseCommandHandler(IWarehouseCommandRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
            : base(mapper, repository, unitOfWork)
        {
        }

        public async Task<Response<WarehouseDto>> Handle(UpdateWarehouseCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null)
                throw new BusinessException("Warehouse not found");

            existing.UpdateName(request.Name);
            existing.ChangeType((Domain.Enums.WarehouseType)request.Type);
            existing.UpdateLocation(request.Location);
            existing.UpdateDescription(request.Description);

            return await ExecuteUpdateAsync<Domain.Entities.Products.Warehouse, WarehouseDto>(
                existing,
                async (w) => await _repo.UpdateAsync(w),
                cancellationToken);
        }
    }
}
