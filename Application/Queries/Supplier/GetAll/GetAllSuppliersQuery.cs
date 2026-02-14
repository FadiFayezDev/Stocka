using Application.Bases;
using Application.Dtos.Purchasing;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Queries.Supplier.GetAll
{
    public class GetAllSuppliersQuery : IRequest<Response<IEnumerable<SupplierDto>>>
    {
    }

    public class GetAllSuppliersQueryHandler : BaseHandler<ISupplierCommandRepository>, IRequestHandler<GetAllSuppliersQuery, Response<IEnumerable<SupplierDto>>>
    {
        public GetAllSuppliersQueryHandler(ISupplierCommandRepository Repository, IMapper mapper) : base(mapper, Repository)
        {
        }

        public async Task<Response<IEnumerable<SupplierDto>>> Handle(GetAllSuppliersQuery request, CancellationToken cancellationToken)
        {
            var items = await _repo.GetAllTableAsync();
            var dtos = _mapper.Map<IEnumerable<SupplierDto>>(items);
            return new Response<IEnumerable<SupplierDto>>(dtos, "Success");
        }
    }
}
