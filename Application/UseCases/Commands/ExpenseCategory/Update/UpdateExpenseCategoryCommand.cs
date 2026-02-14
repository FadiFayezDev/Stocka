using Application.Bases;
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
        public UpdateExpenseCategoryCommandHandler(IExpenseCategoryCommandRepository Repository, IMapper mapper) : base(mapper, Repository)
        {
        }

        public async Task<Response<ExpenseCategoryDto>> Handle(UpdateExpenseCategoryCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null)
                return new Response<ExpenseCategoryDto>("Not found");

            _mapper.Map(request, existing);

             await _repo.UpdateAsync(existing);

            return new Response<ExpenseCategoryDto>();
        }
    }
}