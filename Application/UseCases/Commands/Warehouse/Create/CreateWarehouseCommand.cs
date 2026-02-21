using Application.Bases;
using Application.Common.Interfaces;
using Application.Dtos.Products;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Commands.Warehouse.Create
{
    public class CreateWarehouseCommand : IRequest<Response<WarehouseDto>>
    {
        public Guid BranchId { get; set; }
        public Guid BrandId { get; set; }
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!;
    }

    public class CreateWarehouseCommandHandler : BaseHandler<IWarehouseCommandRepository>, IRequestHandler<CreateWarehouseCommand, Response<WarehouseDto>>
    {
        public CreateWarehouseCommandHandler(IWarehouseCommandRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
            : base(mapper, repository, unitOfWork)
        {
        }

        public async Task<Response<WarehouseDto>> Handle(CreateWarehouseCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Domain.Entities.Products.Warehouse>(request);

            return await ExecuteCreateAsync<Domain.Entities.Products.Warehouse, WarehouseDto>(
                entity,
                async (w) => await _repo.CreateAsync(w),
                cancellationToken);
        }
    }
}
