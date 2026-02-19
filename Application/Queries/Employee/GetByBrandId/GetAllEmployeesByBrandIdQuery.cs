using Application.Bases;
using Application.Dtos.Core;
using Application.QueryRepositories;
using AutoMapper;
using MediatR;

namespace Application.Queries.Employee.GetByBrandId
{
    public class GetAllEmployeesByBrandIdQuery : IRequest<Response<IEnumerable<EmployeeDto>>>
    {
        public Guid BrandId { get; set; }

        public GetAllEmployeesByBrandIdQuery(Guid brandId)
        {
            BrandId = brandId;
        }
    }

    public class GetAllEmployeesByBrandIdQueryHandler : IRequestHandler<GetAllEmployeesByBrandIdQuery, Response<IEnumerable<EmployeeDto>>>
    {
        private readonly IEmployeeQueryRepository _repository;
        private readonly IMapper _mapper;

        public GetAllEmployeesByBrandIdQueryHandler(IEmployeeQueryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<EmployeeDto>>> Handle(GetAllEmployeesByBrandIdQuery request, CancellationToken cancellationToken)
        {
            var employees = await _repository.GetAllByBrandIdAsync(request.BrandId);
            if (employees == null)
                return new Response<IEnumerable<EmployeeDto>>("Employees not found");

            var employeeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);
            return new Response<IEnumerable<EmployeeDto>>(employeeDtos, "Success");
        }
    }
}
