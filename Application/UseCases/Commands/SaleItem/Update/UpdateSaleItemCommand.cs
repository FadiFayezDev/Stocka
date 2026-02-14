using Application.Bases;
using Application.Dtos.Sales;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Commands.SaleItem.Update
{
    public class UpdateSaleItemCommand : IRequest<Response<SaleItemDto>>
    {
        public Guid Id { get; set; }
        public Guid SaleId { get; set; }
        public Guid ProductId { get; set; }
        public Guid BatchId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal CostPrice { get; set; }
    }

    public class UpdateSaleItemCommandHandler : BaseHandler<ISaleItemCommandRepository>, IRequestHandler<UpdateSaleItemCommand, Response<SaleItemDto>>
    {
        public UpdateSaleItemCommandHandler(ISaleItemCommandRepository Repository, IMapper mapper) : base(mapper, Repository)
        {
        }

        public async Task<Response<SaleItemDto>> Handle(UpdateSaleItemCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null)
                return new Response<SaleItemDto>("Not found");

            _mapper.Map(request, existing);

            await _repo.UpdateAsync(existing);

            return new Response<SaleItemDto>();
        }
    }
}
