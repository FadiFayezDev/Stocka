using Application.Bases;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Core;
using Application.QueryRepositories;
using AutoMapper;
using MediatR;

namespace Application.Queries.Employee.GetByBrandId
{
    /// <summary>
    /// The brand ID is injected automatically.
    /// </summary>
    public class GetAllEmployeesByBrandIdQuery : IRequest<Response<IEnumerable<EmployeeDto>>>
    {

    }

    public class GetAllEmployeesByBrandIdQueryHandler : IRequestHandler<GetAllEmployeesByBrandIdQuery, Response<IEnumerable<EmployeeDto>>>
    {
        private readonly IEmployeeQueryRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserContext _currentUser;

        public GetAllEmployeesByBrandIdQueryHandler(IEmployeeQueryRepository repository, IMapper mapper, ICurrentUserContext currentUser)
        {
            _repository = repository;
            _mapper = mapper; 
            _currentUser = currentUser;

        }

        public async Task<Response<IEnumerable<EmployeeDto>>> Handle(GetAllEmployeesByBrandIdQuery request, CancellationToken cancellationToken)
        {
            var brandId = _currentUser.ActiveBrandId;
            
            var employees = await _repository.GetAllByBrandIdAsync(brandId);
            if (employees == null)
                return new Response<IEnumerable<EmployeeDto>>("Employees not found");

            var employeeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);
            return new Response<IEnumerable<EmployeeDto>>(employeeDtos, "Success");
        }
    }
}
