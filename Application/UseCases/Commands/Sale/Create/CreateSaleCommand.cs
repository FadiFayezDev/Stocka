using Application.Bases;
using Application.Dtos.Sales;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Commands.Sale.Create
{
    public class CreateSaleCommand : IRequest<Response<SaleDto>>
    {
        public Guid BrandId { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid? CustomerId { get; set; }
        public DateTime SaleDate { get; set; }
        public string Status { get; set; } = null!;
        public decimal TotalAmount { get; set; }
    }

    public class CreateSaleCommandHandler : BaseHandler<ISaleCommandRepository>, IRequestHandler<CreateSaleCommand, Response<SaleDto>>
    {
        public CreateSaleCommandHandler(ISaleCommandRepository Repository, IMapper mapper) : base(mapper, Repository)
        {
        }

        public async Task<Response<SaleDto>> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Domain.Entities.Sales.Sale>(request);

            await _repo.CreateAsync(entity);
            return new Response<SaleDto>();
        }
    }
}
