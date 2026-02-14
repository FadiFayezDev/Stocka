using Application.Bases;
using Application.Dtos.Sales;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Commands.SaleItem.Create
{
    public class CreateSaleItemCommand : IRequest<Response<SaleItemDto>>
    {
        public Guid SaleId { get; set; }
        public Guid ProductId { get; set; }
        public Guid BatchId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal CostPrice { get; set; }
    }

    public class CreateSaleItemCommandHandler : BaseHandler<ISaleItemCommandRepository>, IRequestHandler<CreateSaleItemCommand, Response<SaleItemDto>>
    {
        public CreateSaleItemCommandHandler(ISaleItemCommandRepository Repository, IMapper mapper) : base(mapper, Repository)
        {
        }

        public async Task<Response<SaleItemDto>> Handle(CreateSaleItemCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Domain.Entities.Sales.SaleItem>(request);

            await _repo.CreateAsync(entity);

            return new Response<SaleItemDto>();
        }
    }
}
