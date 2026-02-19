using Application.Bases;
using Application.Common.Interfaces;
using Application.Dtos.Expenses;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Commands.ExpenseCategory.Create
{
    public class CreateExpenseCategoryCommand : IRequest<Response<ExpenseCategoryDto>>
    {
        public Guid BrandId { get; set; }
        public string Name { get; set; } = null!;
    }

    public class CreateExpenseCategoryCommandHandler : BaseHandler<IExpenseCategoryCommandRepository>, IRequestHandler<CreateExpenseCategoryCommand, Response<ExpenseCategoryDto>>
    {
        public CreateExpenseCategoryCommandHandler(IExpenseCategoryCommandRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
            : base(mapper, repository, unitOfWork)
        {
        }

        public async Task<Response<ExpenseCategoryDto>> Handle(CreateExpenseCategoryCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Domain.Entities.Expenses.ExpenseCategory>(request);

            return await ExecuteCreateAsync<Domain.Entities.Expenses.ExpenseCategory, ExpenseCategoryDto>(
                entity,
                async (ec) => await _repo.CreateAsync(ec),
                cancellationToken);
        }
    }
}
