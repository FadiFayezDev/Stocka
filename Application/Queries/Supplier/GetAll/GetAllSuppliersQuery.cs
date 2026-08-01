using Application.Bases;
using Application.Common.Interfaces;
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
        private readonly ICurrentUserContext _currentUser;

        public GetAllSuppliersQueryHandler(ISupplierCommandRepository Repository, IMapper mapper, ICurrentUserContext currentUser) : base(mapper, Repository)
        {
            _currentUser = currentUser;
        }

        public async Task<Response<IEnumerable<SupplierDto>>> Handle(GetAllSuppliersQuery request, CancellationToken cancellationToken)
        {
            var brandId = _currentUser.ActiveBrandId;

            var items = await _repo.GetAllTableAsync();
            var dtos = _mapper.Map<IEnumerable<SupplierDto>>(items);
            return new Response<IEnumerable<SupplierDto>>(dtos, "Success");
        }
    }
}
