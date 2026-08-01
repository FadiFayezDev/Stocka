using Application.Bases;
using Application.Dtos.Products;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Queries.WarehouseBatch.GetById
{
    public class GetWarehouseBatchByIdQuery : IRequest<Response<WarehouseBatchDto>>
    {
        public Guid Id { get; set; }
    }

    public class GetWarehouseBatchByIdQueryHandler : BaseHandler<IWarehouseBatchCommandRepository>, IRequestHandler<GetWarehouseBatchByIdQuery, Response<WarehouseBatchDto>>
    {
        public GetWarehouseBatchByIdQueryHandler(IWarehouseBatchCommandRepository warehouseBatchRepository, IMapper mapper) : base(mapper, warehouseBatchRepository)
        {
        }

        public async Task<Response<WarehouseBatchDto>> Handle(GetWarehouseBatchByIdQuery request, CancellationToken cancellationToken)
        {
            var warehouseBatch = await _repo.GetByIdAsync(request.Id);
            if (warehouseBatch == null)
                return new Response<WarehouseBatchDto>("WarehouseBatch not found");

            var warehouseBatchDto = _mapper.Map<WarehouseBatchDto>(warehouseBatch);
            return new Response<WarehouseBatchDto>(warehouseBatchDto, "Success");
        }
    }
}
