using Application.Bases;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Core;
using AutoMapper;
using Domain.Contracts;
using Domain.Entities.Core;
using Domain.Primitives;
using MediatR;

namespace Application.UseCases.EmployeeCases
{
    public class HireEmployeeCommand : IRequest<Response<EmployeeDto>>
    {
        public Guid ApplicationUserId { get; set; }
        public Guid BrandId { get; set; }
        public Guid? BranchId { get; set; }
        public string JobTitle { get; set; } = null!;
        public decimal? Salary { get; set; }
        public DateTime HireDate { get; set; }
        public bool IsActive { get; set; }
    }

    public class HireEmployeeCommandHandler : BaseHandler<IEmployeeCommandRepository>, IRequestHandler<HireEmployeeCommand, Response<EmployeeDto>>
    {
        private readonly ICurrentUserContext _currentUser;
        public HireEmployeeCommandHandler(IEmployeeCommandRepository repository, IMapper mapper, IUnitOfWork unitOfWork, ICurrentUserContext currentUser)
            : base(mapper, repository, unitOfWork)
        {
            _currentUser = currentUser;
        }

        public async Task<Response<EmployeeDto>> Handle(HireEmployeeCommand request, CancellationToken cancellationToken)
        {
            var brandId = _currentUser.ActiveBrandId; 
            var branchId = _currentUser.ActiveBranchId;

            if (branchId == null || branchId == Guid.Empty)
                throw new BadRequestException("Active branch is required to create an employee.");

            var employee = new Domain.Entities.Core.Employee(
                request.ApplicationUserId,
                new BrandId(brandId),
                request.JobTitle,
                request.Salary,
                new BranchId(branchId.Value),
                request.HireDate);

            return await ExecuteCreateAsync<Domain.Entities.Core.Employee, EmployeeDto>(
                employee,
                async (emp) => await _repo.CreateAsync(emp),
                cancellationToken);
        }
    }
}