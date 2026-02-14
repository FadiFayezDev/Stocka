using Application.Bases;
using Application.Dtos.Sales;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Commands.Sale.Update
{
    public class UpdateSaleCommand : IRequest<Response<SaleDto>>
    {
        public Guid Id { get; set; }
        public Guid BrandId { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid? CustomerId { get; set; }
        public DateTime SaleDate { get; set; }
        public string Status { get; set; } = null!;
        public decimal TotalAmount { get; set; }
    }

    public class UpdateSaleCommandHandler : BaseHandler<ISaleCommandRepository>, IRequestHandler<UpdateSaleCommand, Response<SaleDto>>
    {
        public UpdateSaleCommandHandler(ISaleCommandRepository Repository, IMapper mapper) : base(mapper, Repository)
        {
        }

        public async Task<Response<SaleDto>> Handle(UpdateSaleCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null)
                return new Response<SaleDto>("Not found");

            _mapper.Map(request, existing);

           await _repo.UpdateAsync(existing);
            return new Response<SaleDto>();
        }
    }
}
