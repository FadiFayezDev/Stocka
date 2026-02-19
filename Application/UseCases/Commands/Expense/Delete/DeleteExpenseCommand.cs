using Application.Bases;
using Application.Common.Interfaces;
using Application.Dtos.Expenses;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Commands.Expense.Delete
{
    public class DeleteExpenseCommand : IRequest<Response<bool>>
    {
        public Guid Id { get; set; }
    }

    public class DeleteExpenseCommandHandler : BaseHandler<IExpenseCommandRepository>, IRequestHandler<DeleteExpenseCommand, Response<bool>>
    {
        public DeleteExpenseCommandHandler(IExpenseCommandRepository repository, IUnitOfWork unitOfWork)
            : base(null, repository, unitOfWork)
        {
        }

        public async Task<Response<bool>> Handle(DeleteExpenseCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null)
                return new Response<bool>(false, "Expense not found");

            return await ExecuteDeleteAsync(
                existing,
                async (e) => await _repo.DeleteAsync(e),
                cancellationToken);
        }
    }
}
