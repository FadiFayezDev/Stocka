using Application.Bases;
using Application.Common.Interfaces;
using Application.Dtos.Core;
using Domain.Contracts;
using MediatR;

namespace Application.UseCases.Commands.Employee.Delete
{
    public class DeleteEmployeeCommand : IRequest<Response<bool>>
    {
        public Guid Id { get; set; }
    }

    public class DeleteEmployeeCommandHandler : BaseHandler<IEmployeeCommandRepository>, IRequestHandler<DeleteEmployeeCommand, Response<bool>>
    {
        public DeleteEmployeeCommandHandler(IEmployeeCommandRepository repository, IUnitOfWork unitOfWork)
            : base(null, repository, unitOfWork)
        {
        }

        public async Task<Response<bool>> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            var existingEmployee = await _repo.GetByIdAsync(request.Id);
            if (existingEmployee == null)
                return new Response<bool>(false, "Employee not found");

            return await ExecuteDeleteAsync(
                existingEmployee,
                async (emp) => await _repo.DeleteAsync(emp),
                cancellationToken);
        }
    }
}
