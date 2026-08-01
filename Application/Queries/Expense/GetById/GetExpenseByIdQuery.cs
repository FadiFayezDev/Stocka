using Application.Bases;
using Application.Dtos.Expenses;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Queries.Expense.GetById
{
    public class GetExpenseByIdQuery : IRequest<Response<ExpenseDto>>
    {
        public Guid Id { get; set; }
    }

    public class GetExpenseByIdQueryHandler : BaseHandler<IExpenseCommandRepository>, IRequestHandler<GetExpenseByIdQuery, Response<ExpenseDto>>
    {
        public GetExpenseByIdQueryHandler(IExpenseCommandRepository Repository, IMapper mapper) : base(mapper, Repository)
        {
        }

        public async Task<Response<ExpenseDto>> Handle(GetExpenseByIdQuery request, CancellationToken cancellationToken)
        {
            var item = await _repo.GetByIdAsync(request.Id);
            if (item == null)
                return new Response<ExpenseDto>("Not found");

            var dto = _mapper.Map<ExpenseDto>(item);
            return new Response<ExpenseDto>(dto, "Success");
        }
    }
}
