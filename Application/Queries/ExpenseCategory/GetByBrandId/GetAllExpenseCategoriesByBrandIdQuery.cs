using Application.Bases;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Expenses;
using Application.QueryRepositories;
using AutoMapper;
using MediatR;

namespace Application.Queries.ExpenseCategory.GetByBrandId
{
    /// <summary>
    /// Get all expense categories by brand id
    /// - The brand ID is injected automatically.
    /// </summary>
    public class GetAllExpenseCategoriesByBrandIdQuery : IRequest<Response<IEnumerable<ExpenseCategoryDto>>>
    {

    }

    public class GetAllExpenseCategoriesByBrandIdQueryHandler : IRequestHandler<GetAllExpenseCategoriesByBrandIdQuery, Response<IEnumerable<ExpenseCategoryDto>>>
    {
        private readonly IExpenseCategoryQueryRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserContext _currentUser;

        public GetAllExpenseCategoriesByBrandIdQueryHandler(IExpenseCategoryQueryRepository repository, IMapper mapper, ICurrentUserContext currentUser)
        {
            _repository = repository;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<Response<IEnumerable<ExpenseCategoryDto>>> Handle(GetAllExpenseCategoriesByBrandIdQuery request, CancellationToken cancellationToken)
        {
            var brandId = _currentUser.ActiveBrandId;

            var expenseCategories = await _repository.GetAllByBrandIdAsync(brandId);
            if (expenseCategories == null)
                return new Response<IEnumerable<ExpenseCategoryDto>>("Expense categories not found");

            var expenseCategoryDtos = _mapper.Map<IEnumerable<ExpenseCategoryDto>>(expenseCategories);
            return new Response<IEnumerable<ExpenseCategoryDto>>(expenseCategoryDtos, "Success");
        }
    }
}
