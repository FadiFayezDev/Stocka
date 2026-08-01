using Application.Bases;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Expenses;
using AutoMapper;
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
        public DeleteExpenseCategoryCommandHandler(IMapper mapper, IExpenseCategoryCommandRepository repository, IUnitOfWork unitOfWork)
            : base(mapper, repository, unitOfWork)
        {
        }

        public async Task<Response<bool>> Handle(DeleteExpenseCategoryCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null)
                throw new BusinessException("Expense category not found");

            return await ExecuteDeleteAsync(
                existing,
                async (ec) => await _repo.DeleteAsync(ec),
                cancellationToken);
        }
    }
}
