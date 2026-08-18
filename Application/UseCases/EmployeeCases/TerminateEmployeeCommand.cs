using Application.Bases;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Core;
using Domain.Contracts;
using MediatR;

namespace Application.UseCases.EmployeeCases
{
    public class TerminateEmployeeCommand : IRequest<Response<bool>>
    {
        public Guid Id { get; set; }
    }

    public class TerminateEmployeeCommandHandler : BaseHandler<IEmployeeCommandRepository>, IRequestHandler<TerminateEmployeeCommand, Response<bool>>
    {
        public TerminateEmployeeCommandHandler(IEmployeeCommandRepository repository, IUnitOfWork unitOfWork)
            : base(null, repository, unitOfWork)
        {
        }

        public async Task<Response<bool>> Handle(TerminateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var existingEmployee = await _repo.GetByIdAsync(request.Id);
            if (existingEmployee == null)
                throw new BusinessException("Employee not found");

            return await ExecuteDeleteAsync(
                existingEmployee,
                async (emp) => await _repo.DeleteAsync(emp),
                cancellationToken);
        }
    }
}
