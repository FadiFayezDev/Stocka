using Application.Bases;
using Application.Dtos.Accounting;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Queries.JournalEntry.GetAll
{
    public class GetAllJournalEntriesQuery : IRequest<Response<IEnumerable<JournalEntryDto>>>
    {
    }

    public class GetAllJournalEntriesQueryHandler : BaseHandler<IJournalEntryCommandRepository>, IRequestHandler<GetAllJournalEntriesQuery, Response<IEnumerable<JournalEntryDto>>>
    {
        public GetAllJournalEntriesQueryHandler(IJournalEntryCommandRepository Repository, IMapper mapper) : base(mapper, Repository)
        {
        }

        public async Task<Response<IEnumerable<JournalEntryDto>>> Handle(GetAllJournalEntriesQuery request, CancellationToken cancellationToken)
        {
            var items = await _repo.GetAllTableAsync();
            var dtos = _mapper.Map<IEnumerable<JournalEntryDto>>(items);
            return new Response<IEnumerable<JournalEntryDto>>(dtos, "Success");
        }
    }
}
