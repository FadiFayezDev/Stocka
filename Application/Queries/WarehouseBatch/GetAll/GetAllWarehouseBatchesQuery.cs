using Application.Bases;
using Application.Dtos.Products;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Queries.WarehouseBatch.GetAll
{
    public class GetAllWarehouseBatchesQuery : IRequest<Response<IEnumerable<WarehouseBatchDto>>>
    {
    }

    public class GetAllWarehouseBatchesQueryHandler : BaseHandler<IWarehouseBatchCommandRepository>, IRequestHandler<GetAllWarehouseBatchesQuery, Response<IEnumerable<WarehouseBatchDto>>>
    {
        public GetAllWarehouseBatchesQueryHandler(IWarehouseBatchCommandRepository warehouseBatchRepository, IMapper mapper) : base(mapper, warehouseBatchRepository)
        {
        }

        public async Task<Response<IEnumerable<WarehouseBatchDto>>> Handle(GetAllWarehouseBatchesQuery request, CancellationToken cancellationToken)
        {
            var warehouseBatches = await _repo.GetAllTableAsync();
            var warehouseBatchDtos = _mapper.Map<IEnumerable<WarehouseBatchDto>>(warehouseBatches);
            return new Response<IEnumerable<WarehouseBatchDto>>(warehouseBatchDtos, "Success");
        }
    }
}
