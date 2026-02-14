using Application.Bases;
using Application.Dtos.Core;
using Application.QueryRepositories;
using AutoMapper;
using MediatR;

namespace Application.Queries.Employee.GetById
{
    public class GetEmployeeByIdQuery : IRequest<Response<EmployeeDto>>
    {
        public Guid Id { get; set; }
    }

    public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, Response<EmployeeDto>>
    {
        private readonly IEmployeeQueryRepository _queryRepository;
        private readonly IMapper _mapper;

        public GetEmployeeByIdQueryHandler(IEmployeeQueryRepository queryRepository, IMapper mapper)
        {
            _queryRepository = queryRepository;
            _mapper = mapper;
        }

        public async Task<Response<EmployeeDto>> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
        {
            var employee = await _queryRepository.GetByIdAsync(request.Id);
            if (employee == null)
                return new Response<EmployeeDto>("Employee not found");

            return new Response<EmployeeDto>(employee, "Success");
        }
    }
}
