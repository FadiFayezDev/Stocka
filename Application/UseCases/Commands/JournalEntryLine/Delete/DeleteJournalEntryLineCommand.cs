using Application.Bases;
using Application.Dtos.Accounting;
using AutoMapper;
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
        public DeleteJournalEntryLineCommandHandler(IJournalEntryLineCommandRepository Repository) : base(null, Repository)
        {
        }

        public async Task<Response<bool>> Handle(DeleteJournalEntryLineCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null)
                return new Response<bool>("Not found");

            await _repo.DeleteAsync(existing);

            return new Response<bool>();
        }
    }
}
