using Application.Bases;
using Application.Dtos.Core;
using AutoMapper;
using Domain.Contracts;
using MediatR;

namespace Application.UseCases.Commands.Employee.Update
{
    public class UpdateEmployeeCommand : IRequest<Response<EmployeeDto>>
    {
        public Guid Id { get; set; }
        public Guid ApplicationUserId { get; set; }
        public Guid BrandId { get; set; }
        public string JobTitle { get; set; } = null!;
        public decimal? Salary { get; set; }
        public DateTime HireDate { get; set; }
        public bool IsActive { get; set; }
    }

    public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, Response<EmployeeDto>>
    {
        private readonly IEmployeeCommandRepository _repository;
        private readonly IMapper _mapper;

        public UpdateEmployeeCommandHandler(IEmployeeCommandRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<EmployeeDto>> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            //var existingEmployee = await _repository.GetByIdAsync(request.Id);
            //if (existingEmployee == null)
            //    return new Response<EmployeeDto>("Employee not found");

            //existingEmployee.UserId = request.ApplicationUserId;
            //existingEmployee.BrandId = request.BrandId;
            //existingEmployee.JobTitle = request.JobTitle;
            //existingEmployee.Salary = request.Salary;
            //existingEmployee.HireDate = request.HireDate;
            //existingEmployee.IsActive = request.IsActive;

            //var updated = await _repository.UpdateAsync(existingEmployee);
            //var employeeDto = _mapper.Map<EmployeeDto>(updated);
            //return new Response<EmployeeDto>(employeeDto, "Updated Successfully");

            throw new NotImplementedException();
        }
    }
}
