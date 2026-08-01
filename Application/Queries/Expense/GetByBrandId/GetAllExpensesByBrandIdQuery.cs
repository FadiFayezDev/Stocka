using Application.Bases;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Expenses;
using Application.QueryRepositories;
using AutoMapper;
using MediatR;

namespace Application.Queries.Expense.GetByBrandId
{
    /// <summary>
    /// Get all expenses by brand id query
    /// - The brand ID is injected automatically.
    /// </summary>
    public class GetAllExpensesByBrandIdQuery : IRequest<Response<IEnumerable<ExpenseDto>>>
    {

    }

    public class GetAllExpensesByBrandIdQueryHandler : IRequestHandler<GetAllExpensesByBrandIdQuery, Response<IEnumerable<ExpenseDto>>>
    {
        private readonly IExpenseQueryRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserContext _currentUser;

        public GetAllExpensesByBrandIdQueryHandler(IExpenseQueryRepository repository, IMapper mapper, ICurrentUserContext currentUser)
        {
            _repository = repository;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<Response<IEnumerable<ExpenseDto>>> Handle(GetAllExpensesByBrandIdQuery request, CancellationToken cancellationToken)
        {
            var brandId = _currentUser.ActiveBrandId;
            var expenses = await _repository.GetAllByBrandIdAsync(brandId);
            if (expenses == null)
                return new Response<IEnumerable<ExpenseDto>>("Expenses not found");

            var expenseDtos = _mapper.Map<IEnumerable<ExpenseDto>>(expenses);
            return new Response<IEnumerable<ExpenseDto>>(expenseDtos, "Success");
        }
    }
}
