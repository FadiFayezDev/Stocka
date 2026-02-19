using Application.Bases;
using Application.Common.Interfaces;
using Application.Dtos.Expenses;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Commands.Expense.Update
{
    public class UpdateExpenseCommand : IRequest<Response<ExpenseDto>>
    {
        public Guid Id { get; set; }
        public Guid BrandId { get; set; }
        public Guid CategoryId { get; set; }
        public decimal Amount { get; set; }
        public DateTime ExpenseDate { get; set; }
    }

    public class UpdateExpenseCommandHandler : BaseHandler<IExpenseCommandRepository>, IRequestHandler<UpdateExpenseCommand, Response<ExpenseDto>>
    {
        public UpdateExpenseCommandHandler(IExpenseCommandRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
            : base(mapper, repository, unitOfWork)
        {
        }

        public async Task<Response<ExpenseDto>> Handle(UpdateExpenseCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null)
                return new Response<ExpenseDto>("Expense not found");

            _mapper.Map(request, existing);

            return await ExecuteUpdateAsync<Domain.Entities.Expenses.Expense, ExpenseDto>(
                existing,
                async (e) => await _repo.UpdateAsync(e),
                cancellationToken);
        }
    }
}
