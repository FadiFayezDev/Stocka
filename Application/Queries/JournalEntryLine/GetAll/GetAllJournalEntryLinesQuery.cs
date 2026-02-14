using Application.Bases;
using Application.Dtos.Accounting;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Queries.JournalEntryLine.GetAll
{
    public class GetAllJournalEntryLinesQuery : IRequest<Response<IEnumerable<JournalEntryLineDto>>>
    {
    }

    public class GetAllJournalEntryLinesQueryHandler : BaseHandler<IJournalEntryLineCommandRepository>, IRequestHandler<GetAllJournalEntryLinesQuery, Response<IEnumerable<JournalEntryLineDto>>>
    {
        public GetAllJournalEntryLinesQueryHandler(IJournalEntryLineCommandRepository Repository, IMapper mapper) : base(mapper, Repository)
        {
        }

        public async Task<Response<IEnumerable<JournalEntryLineDto>>> Handle(GetAllJournalEntryLinesQuery request, CancellationToken cancellationToken)
        {
            var items = await _repo.GetAllTableAsync();
            var dtos = _mapper.Map<IEnumerable<JournalEntryLineDto>>(items);
            return new Response<IEnumerable<JournalEntryLineDto>>(dtos, "Success");
        }
    }
}
