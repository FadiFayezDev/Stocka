using Application.Bases;
using Application.Dtos.Expenses;
using Application.QueryRepositories;
using AutoMapper;
using MediatR;

namespace Application.Queries.Expense.GetByBrandId
{
    public class GetAllExpensesByBrandIdQuery : IRequest<Response<IEnumerable<ExpenseDto>>>
    {
        public Guid BrandId { get; set; }

        public GetAllExpensesByBrandIdQuery(Guid brandId)
        {
            BrandId = brandId;
        }
    }

    public class GetAllExpensesByBrandIdQueryHandler : IRequestHandler<GetAllExpensesByBrandIdQuery, Response<IEnumerable<ExpenseDto>>>
    {
        private readonly IExpenseQueryRepository _repository;
        private readonly IMapper _mapper;

        public GetAllExpensesByBrandIdQueryHandler(IExpenseQueryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<ExpenseDto>>> Handle(GetAllExpensesByBrandIdQuery request, CancellationToken cancellationToken)
        {
            var expenses = await _repository.GetAllByBrandIdAsync(request.BrandId);
            if (expenses == null)
                return new Response<IEnumerable<ExpenseDto>>("Expenses not found");

            var expenseDtos = _mapper.Map<IEnumerable<ExpenseDto>>(expenses);
            return new Response<IEnumerable<ExpenseDto>>(expenseDtos, "Success");
        }
    }
}
