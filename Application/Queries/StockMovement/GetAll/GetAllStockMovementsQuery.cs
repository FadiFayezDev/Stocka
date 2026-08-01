using Application.Bases;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Products;
using Application.QueryRepositories;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Queries.StockMovement.GetAll
{
    public class GetAllStockMovementsQuery : IRequest<Response<IEnumerable<StockMovementDto>>>
    {
    }

    public class GetAllStockMovementsQueryHandler : BaseHandler<IStockMovementQueryRepository>, IRequestHandler<GetAllStockMovementsQuery, Response<IEnumerable<StockMovementDto>>>
    {
        private readonly ICurrentUserContext _currentUser;

        public GetAllStockMovementsQueryHandler(IStockMovementQueryRepository stockMovementRepository, IMapper mapper, ICurrentUserContext currentUser) : base(mapper, stockMovementRepository)
        {
            _currentUser = currentUser;
        }

        public async Task<Response<IEnumerable<StockMovementDto>>> Handle(GetAllStockMovementsQuery request, CancellationToken cancellationToken)
        {
            var brandId = _currentUser.ActiveBrandId;

            var stockMovements = await _repo.GetAllByBrandIdAsync(brandId);
            var stockMovementDtos = _mapper.Map<IEnumerable<StockMovementDto>>(stockMovements);
            return new Response<IEnumerable<StockMovementDto>>(stockMovementDtos, "Success");
        }
    }
}
