using Application.Bases;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Accounting;
using Application.QueryRepositories;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Queries.JournalEntry.GetAll
{
    public class GetAllJournalEntriesQuery : IRequest<Response<IEnumerable<JournalEntryDto>>>
    {
    }

    public class GetAllJournalEntriesQueryHandler : BaseHandler<IJournalEntryQueryRepository>, IRequestHandler<GetAllJournalEntriesQuery, Response<IEnumerable<JournalEntryDto>>>
    {
        private readonly ICurrentUserContext _currentUser;

        public GetAllJournalEntriesQueryHandler(IJournalEntryQueryRepository Repository, IMapper mapper, ICurrentUserContext currentUser) : base(mapper, Repository)
        {
            _currentUser = currentUser;
        }

        public async Task<Response<IEnumerable<JournalEntryDto>>> Handle(GetAllJournalEntriesQuery request, CancellationToken cancellationToken)
        {
            var brandId = _currentUser.ActiveBrandId;

            var items = await _repo.GetAllByBrandIdAsync(brandId);
            var dtos = _mapper.Map<IEnumerable<JournalEntryDto>>(items);
            return new Response<IEnumerable<JournalEntryDto>>(dtos, "Success");
        }
    }
}
