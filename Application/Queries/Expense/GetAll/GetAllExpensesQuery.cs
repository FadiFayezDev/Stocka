using Application.Bases;
using Application.Dtos.Expenses;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Queries.Expense.GetAll
{
    public class GetAllExpensesQuery : IRequest<Response<IEnumerable<ExpenseDto>>>
    {
    }

    public class GetAllExpensesQueryHandler : BaseHandler<IExpenseCommandRepository>, IRequestHandler<GetAllExpensesQuery, Response<IEnumerable<ExpenseDto>>>
    {
        public GetAllExpensesQueryHandler(IExpenseCommandRepository Repository, IMapper mapper) : base(mapper, Repository)
        {
        }

        public async Task<Response<IEnumerable<ExpenseDto>>> Handle(GetAllExpensesQuery request, CancellationToken cancellationToken)
        {
            var items = await _repo.GetAllTableAsync();
            var dtos = _mapper.Map<IEnumerable<ExpenseDto>>(items);
            return new Response<IEnumerable<ExpenseDto>>(dtos, "Success");
        }
    }
}
