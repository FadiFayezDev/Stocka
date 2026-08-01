using Application.Bases;
using Application.Dtos.Products;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Queries.StockMovement.GetById
{
    public class GetStockMovementByIdQuery : IRequest<Response<StockMovementDto>>
    {
        public Guid Id { get; set; }
    }

    public class GetStockMovementByIdQueryHandler : BaseHandler<IStockMovementCommandRepository>, IRequestHandler<GetStockMovementByIdQuery, Response<StockMovementDto>>
    {
        public GetStockMovementByIdQueryHandler(IStockMovementCommandRepository stockMovementRepository, IMapper mapper) : base(mapper, stockMovementRepository)
        {
        }

        public async Task<Response<StockMovementDto>> Handle(GetStockMovementByIdQuery request, CancellationToken cancellationToken)
        {
            var stockMovement = await _repo.GetByIdAsync(request.Id);
            if (stockMovement == null)
                return new Response<StockMovementDto>("StockMovement not found");

            var stockMovementDto = _mapper.Map<StockMovementDto>(stockMovement);
            return new Response<StockMovementDto>(stockMovementDto, "Success");
        }
    }
}
