using Application.Bases;
using Application.Dtos.Purchasing;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Queries.Supplier.GetById
{
    public class GetSupplierByIdQuery : IRequest<Response<SupplierDto>>
    {
        public Guid Id { get; set; }
    }

    public class GetSupplierByIdQueryHandler : BaseHandler<ISupplierCommandRepository>, IRequestHandler<GetSupplierByIdQuery, Response<SupplierDto>>
    {
        public GetSupplierByIdQueryHandler(ISupplierCommandRepository Repository, IMapper mapper) : base(mapper, Repository)
        {
        }

        public async Task<Response<SupplierDto>> Handle(GetSupplierByIdQuery request, CancellationToken cancellationToken)
        {
            var item = await _repo.GetByIdAsync(request.Id);
            if (item == null)
                return new Response<SupplierDto>("Not found");

            var dto = _mapper.Map<SupplierDto>(item);
            return new Response<SupplierDto>(dto, "Success");
        }
    }
}
