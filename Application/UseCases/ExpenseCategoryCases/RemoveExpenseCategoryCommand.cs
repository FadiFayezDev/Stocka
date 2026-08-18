using Application.Bases;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Expenses;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;


namespace Application.UseCases.ExpenseCategoryCases
{
    public class RemoveExpenseCategoryCommand : IRequest<Response<bool>>
    {
        public Guid Id { get; set; }
    }

    public class RemoveExpenseCategoryCommandHandler : BaseHandler<IExpenseCategoryCommandRepository>, IRequestHandler<RemoveExpenseCategoryCommand, Response<bool>>
    {
        public RemoveExpenseCategoryCommandHandler(IMapper mapper, IExpenseCategoryCommandRepository repository, IUnitOfWork unitOfWork)
            : base(mapper, repository, unitOfWork)
        {
        }

        public async Task<Response<bool>> Handle(RemoveExpenseCategoryCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null)
                throw new BusinessException("Expense category not found");

            return await ExecuteDeleteAsync(
                existing,
                async (ec) => await _repo.DeleteAsync(ec),
                cancellationToken);
        }
    }
}
