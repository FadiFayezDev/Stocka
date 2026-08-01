using Application.Bases;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Products;
using Application.QueryRepositories;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Queries.WarehouseBatch.GetAll
{
    public class GetAllWarehouseBatchesQuery : IRequest<Response<IEnumerable<WarehouseBatchDto>>>
    {
    }

    public class GetAllWarehouseBatchesQueryHandler : BaseHandler<IWarehouseBatchQueryRepository>, IRequestHandler<GetAllWarehouseBatchesQuery, Response<IEnumerable<WarehouseBatchDto>>>
    {
        private readonly ICurrentUserContext _currentUser;

        public GetAllWarehouseBatchesQueryHandler(IMapper mapper, IWarehouseBatchQueryRepository warehouseBatchRepository, ICurrentUserContext currentUser) : base(mapper, warehouseBatchRepository)
        {
            _currentUser = currentUser;
        }

        public async Task<Response<IEnumerable<WarehouseBatchDto>>> Handle(GetAllWarehouseBatchesQuery request, CancellationToken cancellationToken)
        {
            var brandId = _currentUser.ActiveBrandId;

            var warehouseBatches = await _repo.GetAllByBrandIdAsync(brandId);
            var warehouseBatchDtos = _mapper.Map<IEnumerable<WarehouseBatchDto>>(warehouseBatches);
            return new Response<IEnumerable<WarehouseBatchDto>>(warehouseBatchDtos, "Success");
        }
    }
}
