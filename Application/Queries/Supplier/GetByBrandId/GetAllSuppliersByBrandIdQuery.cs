using Application.Bases;
using Application.Dtos.Purchasing;
using Application.QueryRepositories;
using AutoMapper;
using MediatR;

namespace Application.Queries.Supplier.GetByBrandId
{
    public class GetAllSuppliersByBrandIdQuery : IRequest<Response<IEnumerable<SupplierDto>>>
    {
        public Guid BrandId { get; set; }

        public GetAllSuppliersByBrandIdQuery(Guid brandId)
        {
            BrandId = brandId;
        }
    }

    public class GetAllSuppliersByBrandIdQueryHandler : IRequestHandler<GetAllSuppliersByBrandIdQuery, Response<IEnumerable<SupplierDto>>>
    {
        private readonly ISupplierQueryRepository _repository;
        private readonly IMapper _mapper;

        public GetAllSuppliersByBrandIdQueryHandler(ISupplierQueryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<SupplierDto>>> Handle(GetAllSuppliersByBrandIdQuery request, CancellationToken cancellationToken)
        {
            var suppliers = await _repository.GetAllByBrandIdAsync(request.BrandId);
            if (suppliers == null)
                return new Response<IEnumerable<SupplierDto>>("Suppliers not found");

            var supplierDtos = _mapper.Map<IEnumerable<SupplierDto>>(suppliers);
            return new Response<IEnumerable<SupplierDto>>(supplierDtos, "Success");
        }
    }
}
