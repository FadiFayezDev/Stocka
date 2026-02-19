using Application.Bases;
using Application.Common.Interfaces;
using Application.Dtos.Accounting;
using Application.QueryRepositories;
using Domain.Contracts;
using MediatR;

namespace Application.UseCases.Commands.Account.Delete
{
    public class DeleteAccountCommand : IRequest<Response<bool>>
    {
        public Guid Id { get; set; }
    }

    public class DeleteAccountCommandHandler : BaseHandler<IAccountCommandRepository, IAccountQueryRepository>, IRequestHandler<DeleteAccountCommand, Response<bool>>
    {
        public DeleteAccountCommandHandler(IAccountCommandRepository command, IAccountQueryRepository query, IUnitOfWork work)
            : base(null, command, query, work)
        {
        }

        public async Task<Response<bool>> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
        {
            var existing = await _command.GetByIdAsync(request.Id);
            if (existing == null)
                return new Response<bool>(false, "Account not found");

            return await ExecuteDeleteAsync(
                existing,
                async (acc) => await _command.DeleteAsync(acc),
                cancellationToken
            );
        }
    }
}