using Application.Bases;
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
        public Guid BranchId { get; set; }
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!;
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
                return new Response<WarehouseDto>("Warehouse not found");

            _mapper.Map(request, existing);

            return await ExecuteUpdateAsync<Domain.Entities.Products.Warehouse, WarehouseDto>(
                existing,
                async (w) => await _repo.UpdateAsync(w),
                cancellationToken);
        }
    }
}
