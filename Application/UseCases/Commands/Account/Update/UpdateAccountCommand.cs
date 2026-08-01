using Application.Bases;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Accounting;
using Application.QueryRepositories;
using AutoMapper;
using Domain.Contracts;
using MediatR;

namespace Application.UseCases.Commands.Account.Update
{
    public class UpdateAccountCommand : IRequest<Response<AccountDto>>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public int Type { get; set; }
    }

    public class UpdateAccountCommandHandler : BaseHandler<IAccountCommandRepository>, IRequestHandler<UpdateAccountCommand, Response<AccountDto>>
    {
        public UpdateAccountCommandHandler(IMapper mapper, IAccountCommandRepository command, IUnitOfWork work)
            : base(mapper, command, work)
        {
        }

        public async Task<Response<AccountDto>> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null)
                throw new BusinessException("Account not found");

            existing.UpdateName(request.Name);
            existing.UpdateType((Domain.Enums.AccountType)request.Type);

            return await ExecuteUpdateAsync<Domain.Entities.Accounting.Account, AccountDto>(
                existing,
                async (acc) => await _repo.UpdateAsync(acc),
                cancellationToken);
        }
    }
}