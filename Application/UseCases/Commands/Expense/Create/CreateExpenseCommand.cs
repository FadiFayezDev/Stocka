using Application.Bases;
using Application.Common.Interfaces;
using Application.Dtos.Expenses;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Commands.Expense.Create
{
    public class CreateExpenseCommand : IRequest<Response<ExpenseDto>>
    {
        public Guid BrandId { get; set; }
        public Guid CategoryId { get; set; }
        public decimal Amount { get; set; }
        public DateTime ExpenseDate { get; set; }
    }

    public class CreateExpenseCommandHandler : BaseHandler<IExpenseCommandRepository>, IRequestHandler<CreateExpenseCommand, Response<ExpenseDto>>
    {
        public CreateExpenseCommandHandler(IExpenseCommandRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
            : base(mapper, repository, unitOfWork)
        {
        }

        public async Task<Response<ExpenseDto>> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Domain.Entities.Expenses.Expense>(request);

            return await ExecuteCreateAsync<Domain.Entities.Expenses.Expense, ExpenseDto>(
                entity,
                async (e) => await _repo.CreateAsync(e),
                cancellationToken);
        }
    }
}