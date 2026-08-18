using Application.Bases;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Accounting;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.UseCases.JournalEntryCases
{
    public class UpdateJournalEntryCommand : IRequest<Response<JournalEntryDto>>
    {
        public Guid Id { get; set; }
        public DateTime EntryDate { get; set; }
        public string? Description { get; set; }
    }

    public class UpdateJournalEntryCommandHandler : BaseHandler<IJournalEntryCommandRepository>, IRequestHandler<UpdateJournalEntryCommand, Response<JournalEntryDto>>
    {
        public UpdateJournalEntryCommandHandler(IJournalEntryCommandRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
            : base(mapper, repository, unitOfWork)
        {
        }

        public async Task<Response<JournalEntryDto>> Handle(UpdateJournalEntryCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null)
                throw new BusinessException("Journal entry not found");

            existing.UpdateDescription(request.Description);
            existing.UpdateEntryDate(request.EntryDate);

            return await ExecuteUpdateAsync<Domain.Entities.Accounting.JournalEntry, JournalEntryDto>(
                existing,
                async (je) => await _repo.UpdateAsync(je),
                cancellationToken);
        }
    }
}