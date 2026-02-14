using Application.Bases;
using Application.Dtos.Expenses;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Queries.ExpenseCategory.GetAll
{
    public class GetAllExpenseCategoriesQuery : IRequest<Response<IEnumerable<ExpenseCategoryDto>>>
    {
    }

    public class GetAllExpenseCategoriesQueryHandler : BaseHandler<IExpenseCategoryCommandRepository>, IRequestHandler<GetAllExpenseCategoriesQuery, Response<IEnumerable<ExpenseCategoryDto>>>
    {
        public GetAllExpenseCategoriesQueryHandler(IExpenseCategoryCommandRepository Repository, IMapper mapper) : base(mapper, Repository)
        {
        }

        public async Task<Response<IEnumerable<ExpenseCategoryDto>>> Handle(GetAllExpenseCategoriesQuery request, CancellationToken cancellationToken)
        {
            var items = await _repo.GetAllTableAsync();
            var dtos = _mapper.Map<IEnumerable<ExpenseCategoryDto>>(items);
            return new Response<IEnumerable<ExpenseCategoryDto>>(dtos, "Success");
        }
    }
}
