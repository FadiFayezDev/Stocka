using Application.Bases;
using Application.Common.Interfaces;
using Application.Dtos.Expenses;
using Domain.Repositories.Commands;
using MediatR;


namespace Application.Features.Commands.ExpenseCategory.Delete
{
    public class DeleteExpenseCategoryCommand : IRequest<Response<bool>>
    {
        public Guid Id { get; set; }
    }

    public class DeleteExpenseCategoryCommandHandler : BaseHandler<IExpenseCategoryCommandRepository>, IRequestHandler<DeleteExpenseCategoryCommand, Response<bool>>
    {
        public DeleteExpenseCategoryCommandHandler(IExpenseCategoryCommandRepository repository, IUnitOfWork unitOfWork)
            : base(null, repository, unitOfWork)
        {
        }

        public async Task<Response<bool>> Handle(DeleteExpenseCategoryCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null)
                return new Response<bool>(false, "Expense category not found");

            return await ExecuteDeleteAsync(
                existing,
                async (ec) => await _repo.DeleteAsync(ec),
                cancellationToken);
        }
    }
}
