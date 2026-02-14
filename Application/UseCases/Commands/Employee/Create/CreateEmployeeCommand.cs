using Application.Bases;
using Application.Dtos.Core;
using AutoMapper;
using Domain.Contracts;
using MediatR;

namespace Application.UseCases.Commands.Employee.Create
{
    public class CreateEmployeeCommand : IRequest<Response<EmployeeDto>>
    {
        public Guid ApplicationUserId { get; set; }
        public Guid BrandId { get; set; }
        public string JobTitle { get; set; } = null!;
        public decimal? Salary { get; set; }
        public DateTime HireDate { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, Response<EmployeeDto>>
    {
        private readonly IEmployeeCommandRepository _repository;
        private readonly IMapper _mapper;

        public CreateEmployeeCommandHandler(IEmployeeCommandRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<EmployeeDto>> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            //var employee = new Domain.Entities.Core.Employee
            //{
            //    UserId = request.ApplicationUserId,
            //    BrandId = request.BrandId,
            //    JobTitle = request.JobTitle,
            //    Salary = request.Salary,
            //    HireDate = request.HireDate,
            //    IsActive = request.IsActive
            //};

            //var created = await _repository.AddAsync(employee);
            //var employeeDto = _mapper.Map<EmployeeDto>(created);
            //return new Response<EmployeeDto>(employeeDto, "Created Successfully"); 
            throw new NotImplementedException();
        }
    }
}