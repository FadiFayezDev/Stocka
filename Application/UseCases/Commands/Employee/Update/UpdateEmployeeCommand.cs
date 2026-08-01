using Application.Bases;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Core;
using AutoMapper;
using Domain.Contracts;
using Domain.Primitives;
using MediatR;

namespace Application.UseCases.Commands.Employee.Update
{
    public class UpdateEmployeeCommand : IRequest<Response<EmployeeDto>>
    {
        public Guid Id { get; set; }
        public Guid ApplicationUserId { get; set; }
        public Guid BranchId { get; set; }
        public string JobTitle { get; set; } = null!;
        public decimal Salary { get; set; }
        public DateTime HireDate { get; set; }
        public bool IsActive { get; set; }
    }

    public class UpdateEmployeeCommandHandler : BaseHandler<IEmployeeCommandRepository>, IRequestHandler<UpdateEmployeeCommand, Response<EmployeeDto>>
    {
        private readonly ICurrentUserContext _currentUser;
        public UpdateEmployeeCommandHandler(
            IEmployeeCommandRepository repository, 
            IMapper mapper, 
            IUnitOfWork unitOfWork, 
            ICurrentUserContext currentUser)
            : base(mapper, repository, unitOfWork)
        {
            _currentUser = currentUser;
        }

        public async Task<Response<EmployeeDto>> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var brandId = _currentUser.ActiveBrandId;

            if (brandId == null || brandId == Guid.Empty)
                throw new BadRequestException("Active brand is required to create an expense.");

            var existingEmployee = await _repo.GetByIdAsync(request.Id);
            if (existingEmployee == null)
                throw new BusinessException("Employee not found");

            existingEmployee.UpdateJobTitle(request.JobTitle);
            existingEmployee.UpdateSalary(request.Salary);
            existingEmployee.AssignToBranch(new BranchId(request.BranchId));

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