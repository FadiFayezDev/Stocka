using Application.Bases;
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
        public CreateExpenseCommandHandler(IExpenseCommandRepository Repository, IMapper mapper) : base(mapper, Repository)
        {
        }

        public async Task<Response<ExpenseDto>> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Domain.Entities.Expenses.Expense>(request);

            await _repo.CreateAsync(entity);

            return new Response<ExpenseDto>();
        }
    }
}