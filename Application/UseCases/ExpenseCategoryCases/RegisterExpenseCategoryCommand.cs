using Application.Bases;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Expenses;
using AutoMapper;
using Domain.Entities.Expenses;
using Domain.Primitives;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.UseCases.ExpenseCategoryCases
{
    public class RegisterExpenseCategoryCommand : IRequest<Response<ExpenseCategoryDto>>
    {
        public string Name { get; set; } = null!;
    }

    public class RegisterExpenseCategoryCommandHandler : BaseHandler<IExpenseCategoryCommandRepository>, IRequestHandler<RegisterExpenseCategoryCommand, Response<ExpenseCategoryDto>>
    {
        private readonly ICurrentUserContext _currentUser;
        public RegisterExpenseCategoryCommandHandler(IExpenseCategoryCommandRepository repository, IMapper mapper, IUnitOfWork unitOfWork, ICurrentUserContext currentUser)
            : base(mapper, repository, unitOfWork)
        {
            _currentUser = currentUser;
        }

        public async Task<Response<ExpenseCategoryDto>> Handle(RegisterExpenseCategoryCommand request, CancellationToken cancellationToken)
        {
            var brandId = _currentUser.ActiveBrandId;

            var entity = new Domain.Entities.Expenses.ExpenseCategory(new BrandId(brandId), request.Name);

            return await ExecuteCreateAsync<Domain.Entities.Expenses.ExpenseCategory, ExpenseCategoryDto>(
                entity,
                async (ec) => await _repo.CreateAsync(ec),
                cancellationToken);
        }
    }
}
