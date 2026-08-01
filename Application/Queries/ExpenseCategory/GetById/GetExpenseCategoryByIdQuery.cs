using Application.Bases;
using Application.Dtos.Expenses;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Queries.ExpenseCategory.GetById
{
    public class GetExpenseCategoryByIdQuery : IRequest<Response<ExpenseCategoryDto>>
    {
        public Guid Id { get; set; }
    }

    public class GetExpenseCategoryByIdQueryHandler : BaseHandler<IExpenseCategoryCommandRepository>, IRequestHandler<GetExpenseCategoryByIdQuery, Response<ExpenseCategoryDto>>
    {
        public GetExpenseCategoryByIdQueryHandler(IExpenseCategoryCommandRepository Repository, IMapper mapper) : base(mapper, Repository)
        {
        }

        public async Task<Response<ExpenseCategoryDto>> Handle(GetExpenseCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var item = await _repo.GetByIdAsync(request.Id);
            if (item == null)
                return new Response<ExpenseCategoryDto>("Not found");

            var dto = _mapper.Map<ExpenseCategoryDto>(item);
            return new Response<ExpenseCategoryDto>(dto, "Success");
        }
    }
}
