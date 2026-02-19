using Application.Bases;
using Application.Common.Interfaces;
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

    public class UpdateEmployeeCommandHandler : BaseHandler<IEmployeeCommandRepository>, IRequestHandler<UpdateEmployeeCommand, Response<EmployeeDto>>
    {
        public UpdateEmployeeCommandHandler(IEmployeeCommandRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
            : base(mapper, repository, unitOfWork)
        {
        }

        public async Task<Response<EmployeeDto>> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var existingEmployee = await _repo.GetByIdAsync(request.Id);
            if (existingEmployee == null)
                return new Response<EmployeeDto>("Employee not found");

            existingEmployee.UpdateJobTitle(request.JobTitle);
            if (request.Salary.HasValue)
                existingEmployee.UpdateSalary(request.Salary.Value);

            if (request.IsActive && !existingEmployee.IsActive)
                existingEmployee.Activate();
            else if (!request.IsActive && existingEmployee.IsActive)
                existingEmployee.Deactivate();

            return await ExecuteUpdateAsync<Domain.Entities.Core.Employee, EmployeeDto>(
                existingEmployee,
                async (emp) => await _repo.UpdateAsync(emp),
                cancellationToken);
        }
    }
}
