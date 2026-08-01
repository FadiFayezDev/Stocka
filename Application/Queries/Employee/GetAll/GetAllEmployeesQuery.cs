using Application.Bases;
using Application.Dtos.Core;
using Application.QueryRepositories;
using AutoMapper;
using MediatR;

namespace Application.Queries.Employee.GetAll
{
    public class GetAllEmployeesQuery : IRequest<Response<IEnumerable<EmployeeDto>>>
    {
    }

    public class GetAllEmployeesQueryHandler : IRequestHandler<GetAllEmployeesQuery, Response<IEnumerable<EmployeeDto>>>
    {
        private readonly IEmployeeQueryRepository _queryRepository;
        private readonly IMapper _mapper;

        public GetAllEmployeesQueryHandler(IEmployeeQueryRepository queryRepository, IMapper mapper)
        {
            _queryRepository = queryRepository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<EmployeeDto>>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
        {
            var employees = await _queryRepository.GetAllTableAsync();
            var employeeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);
            return new Response<IEnumerable<EmployeeDto>>(employeeDtos, "Success");
        }
    }
}
