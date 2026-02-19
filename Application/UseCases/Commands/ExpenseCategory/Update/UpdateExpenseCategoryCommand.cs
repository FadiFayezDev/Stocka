using Application.Bases;
using Application.Common.Interfaces;
using Application.Dtos.Expenses;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Commands.ExpenseCategory.Update
{
    public class UpdateExpenseCategoryCommand : IRequest<Response<ExpenseCategoryDto>>
    {
        public Guid Id { get; set; }
        public Guid BrandId { get; set; }
        public string Name { get; set; } = null!;
    }

    public class UpdateExpenseCategoryCommandHandler : BaseHandler<IExpenseCategoryCommandRepository>, IRequestHandler<UpdateExpenseCategoryCommand, Response<ExpenseCategoryDto>>
    {
        public UpdateExpenseCategoryCommandHandler(IExpenseCategoryCommandRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
            : base(mapper, repository, unitOfWork)
        {
        }

        public async Task<Response<ExpenseCategoryDto>> Handle(UpdateExpenseCategoryCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null)
                return new Response<ExpenseCategoryDto>("Expense category not found");

            _mapper.Map(request, existing);

            return await ExecuteUpdateAsync<Domain.Entities.Expenses.ExpenseCategory, ExpenseCategoryDto>(
                existing,
                async (ec) => await _repo.UpdateAsync(ec),
                cancellationToken);
        }
    }
}