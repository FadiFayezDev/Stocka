using Application.Bases;
using Application.Dtos.Products;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Queries.StockMovement.GetAll
{
    public class GetAllStockMovementsQuery : IRequest<Response<IEnumerable<StockMovementDto>>>
    {
    }

    public class GetAllStockMovementsQueryHandler : BaseHandler<IStockMovementCommandRepository>, IRequestHandler<GetAllStockMovementsQuery, Response<IEnumerable<StockMovementDto>>>
    {
        public GetAllStockMovementsQueryHandler(IStockMovementCommandRepository stockMovementRepository, IMapper mapper) : base(mapper, stockMovementRepository)
        {
        }

        public async Task<Response<IEnumerable<StockMovementDto>>> Handle(GetAllStockMovementsQuery request, CancellationToken cancellationToken)
        {
            var stockMovements = await _repo.GetAllTableAsync();
            var stockMovementDtos = _mapper.Map<IEnumerable<StockMovementDto>>(stockMovements);
            return new Response<IEnumerable<StockMovementDto>>(stockMovementDtos, "Success");
        }
    }
}
