using Application.Bases;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Accounting;
using Application.QueryRepositories;
using Domain.Contracts;
using MediatR;

namespace Application.UseCases.Account
{
    public class CloseAccountCommand : IRequest<Response<bool>>
    {
        public Guid Id { get; set; }
    }

    public class CloseAccountCommandHandler : BaseHandler<IAccountCommandRepository, IAccountQueryRepository>, IRequestHandler<CloseAccountCommand, Response<bool>>
    {
        public CloseAccountCommandHandler(IAccountCommandRepository command, IAccountQueryRepository query, IUnitOfWork work)
            : base(command, query, work)
        {
        }

        public async Task<Response<bool>> Handle(CloseAccountCommand request, CancellationToken cancellationToken)
        {
            var existing = await _command.GetByIdAsync(request.Id);
            if (existing == null)
                throw new BusinessException("Account not found");

            return await ExecuteDeleteAsync(
                existing,
                async (acc) => await _command.DeleteAsync(acc),
                cancellationToken
            );
        }
    }
}