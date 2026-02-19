using Application.Bases;
using Application.Common.Interfaces;
using Application.Dtos.Accounting;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Commands.JournalEntryLine.Delete
{
    public class DeleteJournalEntryLineCommand : IRequest<Response<bool>>
    {
        public Guid Id { get; set; }
    }

    public class DeleteJournalEntryLineCommandHandler : BaseHandler<IJournalEntryLineCommandRepository>, IRequestHandler<DeleteJournalEntryLineCommand, Response<bool>>
    {
        public DeleteJournalEntryLineCommandHandler(IJournalEntryLineCommandRepository repository, IUnitOfWork unitOfWork)
            : base(null, repository, unitOfWork)
        {
        }

        public async Task<Response<bool>> Handle(DeleteJournalEntryLineCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null)
                return new Response<bool>(false, "Journal entry line not found");

            return await ExecuteDeleteAsync(
                existing,
                async (jel) => await _repo.DeleteAsync(jel),
                cancellationToken);
        }
    }
}
