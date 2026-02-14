using Application.Bases;
using Application.Dtos.Accounting;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Commands.JournalEntry.Create
{
    public class CreateJournalEntryCommand : IRequest<Response<JournalEntryDto>>
    {
        public Guid BrandId { get; set; }
        public DateTime EntryDate { get; set; }
        public string? Description { get; set; }
    }

    public class CreateJournalEntryCommandHandler : BaseHandler<IJournalEntryCommandRepository>, IRequestHandler<CreateJournalEntryCommand, Response<JournalEntryDto>>
    {
        public CreateJournalEntryCommandHandler(IJournalEntryCommandRepository Repository, IMapper mapper) : base(mapper, Repository)
        {
        }

        public async Task<Response<JournalEntryDto>> Handle(CreateJournalEntryCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Domain.Entities.Accounting.JournalEntry>(request);

            await _repo.CreateAsync(entity);
            return new Response<JournalEntryDto>();
        }
    }
}
