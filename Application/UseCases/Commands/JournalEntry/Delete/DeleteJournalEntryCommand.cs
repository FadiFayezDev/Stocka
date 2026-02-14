using Application.Bases;
using Application.Dtos.Accounting;
using AutoMapper;
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
        public DeleteJournalEntryCommandHandler(IJournalEntryCommandRepository Repository) : base(null, Repository)
        {
        }

        public async Task<Response<bool>> Handle(DeleteJournalEntryCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null)
                return new Response<bool>("Not found");

            await _repo.DeleteAsync(existing);
            return new Response<bool>();
        }
    }
}
