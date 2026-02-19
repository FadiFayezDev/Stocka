using Application.Bases;
using Application.Dtos.Expenses;
using Application.QueryRepositories;
using AutoMapper;
using MediatR;

namespace Application.Queries.ExpenseCategory.GetByBrandId
{
    public class GetAllExpenseCategoriesByBrandIdQuery : IRequest<Response<IEnumerable<ExpenseCategoryDto>>>
    {
        public Guid BrandId { get; set; }

        public GetAllExpenseCategoriesByBrandIdQuery(Guid brandId)
        {
            BrandId = brandId;
        }
    }

    public class GetAllExpenseCategoriesByBrandIdQueryHandler : IRequestHandler<GetAllExpenseCategoriesByBrandIdQuery, Response<IEnumerable<ExpenseCategoryDto>>>
    {
        private readonly IExpenseCategoryQueryRepository _repository;
        private readonly IMapper _mapper;

        public GetAllExpenseCategoriesByBrandIdQueryHandler(IExpenseCategoryQueryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<ExpenseCategoryDto>>> Handle(GetAllExpenseCategoriesByBrandIdQuery request, CancellationToken cancellationToken)
        {
            var expenseCategories = await _repository.GetAllByBrandIdAsync(request.BrandId);
            if (expenseCategories == null)
                return new Response<IEnumerable<ExpenseCategoryDto>>("Expense categories not found");

            var expenseCategoryDtos = _mapper.Map<IEnumerable<ExpenseCategoryDto>>(expenseCategories);
            return new Response<IEnumerable<ExpenseCategoryDto>>(expenseCategoryDtos, "Success");
        }
    }
}
