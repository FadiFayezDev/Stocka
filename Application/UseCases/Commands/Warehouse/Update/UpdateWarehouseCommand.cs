using Application.Bases;
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
        public UpdateWarehouseCommandHandler(IWarehouseCommandRepository Repository, IMapper mapper) : base(mapper, Repository)
        {
        }

        public async Task<Response<WarehouseDto>> Handle(UpdateWarehouseCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null)
                return new Response<WarehouseDto>("Not found");

            _mapper.Map(request, existing);

             await _repo.UpdateAsync(existing);

            return new Response<WarehouseDto>();
        }
    }
}
