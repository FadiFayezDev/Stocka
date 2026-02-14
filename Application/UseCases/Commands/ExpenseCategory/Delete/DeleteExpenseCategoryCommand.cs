using Application.Bases;
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
        public DeleteExpenseCategoryCommandHandler(IExpenseCategoryCommandRepository Repository) : base(null, Repository)
        {
        }

        public async Task<Response<bool>> Handle(DeleteExpenseCategoryCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null)
                return new Response<bool>("Not found");

            await _repo.DeleteAsync(existing);

            return new Response<bool>();
        }
    }
}
