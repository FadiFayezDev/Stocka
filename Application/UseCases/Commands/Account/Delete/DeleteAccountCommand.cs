using Application.Bases;
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

    public class DeleteAccountCommandHandler : IRequestHandler<DeleteAccountCommand, Response<bool>>
    {
        private readonly IAccountCommandRepository _commands;
        private readonly IAccountQueryRepository _queries;

        public DeleteAccountCommandHandler(IAccountCommandRepository commands, IAccountQueryRepository queries)
        {
            _commands = commands;
            _queries = queries;
        }

        public async Task<Response<bool>> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
        {
            var existing = await _queries.GetByIdAsync(request.Id);
            if (existing == null)
                return new Response<bool>("Not found");

            //await _commands.DeleteAsync(existing);
            return new Response<bool>(true, "Deleted Successfully");
        }
    }
}