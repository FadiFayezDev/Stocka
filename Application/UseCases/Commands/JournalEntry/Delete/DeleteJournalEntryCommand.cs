using Application.Bases;
using Application.Common.Interfaces;
using Application.Dtos.Accounting;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Commands.JournalEntry.Delete
{
    public class DeleteJournalEntryCommand : IRequest<Response<bool>>
    {
        public Guid Id { get; set; }
    }

    public class DeleteJournalEntryCommandHandler : BaseHandler<IJournalEntryCommandRepository>, IRequestHandler<DeleteJournalEntryCommand, Response<bool>>
    {
        public DeleteJournalEntryCommandHandler(IJournalEntryCommandRepository repository, IUnitOfWork unitOfWork)
            : base(null, repository, unitOfWork)
        {
        }

        public async Task<Response<bool>> Handle(DeleteJournalEntryCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null)
                return new Response<bool>(false, "Journal entry not found");

            return await ExecuteDeleteAsync(
                existing,
                async (je) => await _repo.DeleteAsync(je),
                cancellationToken);
        }
    }
}
