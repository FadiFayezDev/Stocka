using Application.Bases;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Expenses;
using AutoMapper;
using Domain.Entities.Expenses;
using Domain.Primitives;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.UseCases.ExpenseCases
{
    public class RecordExpenseCommand : IRequest<Response<ExpenseDto>>
    {
        public Guid BrandId { get; set; }
        public Guid? BranchId { get; set; }
        public Guid CategoryId { get; set; }
        public decimal Amount { get; set; }
        public DateTime ExpenseDate { get; set; }
    }

    public class RecordExpenseCommandHandler : BaseHandler<IExpenseCommandRepository>, IRequestHandler<RecordExpenseCommand, Response<ExpenseDto>>
    {
        private readonly ICurrentUserContext _currentUser;
        public RecordExpenseCommandHandler(IExpenseCommandRepository repository, IMapper mapper, IUnitOfWork unitOfWork, ICurrentUserContext currentUser)
            : base(mapper, repository, unitOfWork)
        {
            _currentUser = currentUser;
        }

        public async Task<Response<ExpenseDto>> Handle(RecordExpenseCommand request, CancellationToken cancellationToken)
        {
            var brandId = _currentUser.ActiveBrandId;
            var branchId = _currentUser.ActiveBranchId;

            if (branchId == null || branchId == Guid.Empty)
                throw new BadRequestException("Active branch is required to create an expense.");

            var entity = new Domain.Entities.Expenses.Expense(
                new BrandId(brandId),
                new ExpenseCategoryId(request.CategoryId),
                request.Amount,
                new BranchId(branchId.Value),
                request.ExpenseDate);

            return await ExecuteCreateAsync<Domain.Entities.Expenses.Expense, ExpenseDto>(
                entity,
                async (e) => await _repo.CreateAsync(e),
                cancellationToken);
        }
    }
}
