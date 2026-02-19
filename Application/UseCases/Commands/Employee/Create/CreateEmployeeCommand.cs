using Application.Bases;
using Application.Common.Interfaces;
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

    public class CreateEmployeeCommandHandler : BaseHandler<IEmployeeCommandRepository>, IRequestHandler<CreateEmployeeCommand, Response<EmployeeDto>>
    {
        public CreateEmployeeCommandHandler(IEmployeeCommandRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
            : base(mapper, repository, unitOfWork)
        {
        }

        public async Task<Response<EmployeeDto>> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = new Domain.Entities.Core.Employee(
                request.ApplicationUserId,
                request.BrandId,
                request.JobTitle,
                request.Salary,
                request.HireDate);

            return await ExecuteCreateAsync<Domain.Entities.Core.Employee, EmployeeDto>(
                employee,
                async (emp) => await _repo.CreateAsync(emp),
                cancellationToken);
        }
    }
}