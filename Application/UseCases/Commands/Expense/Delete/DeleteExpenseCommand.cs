using Application.Bases;
using Application.Dtos.Expenses;
using AutoMapper;
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
        public DeleteExpenseCommandHandler(IExpenseCommandRepository Repository) : base(null, Repository)
        {
        }

        public async Task<Response<bool>> Handle(DeleteExpenseCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null)
                return new Response<bool>("Not found");

            await _repo.DeleteAsync(existing);

            return new Response<bool>();
        }
    }
}
