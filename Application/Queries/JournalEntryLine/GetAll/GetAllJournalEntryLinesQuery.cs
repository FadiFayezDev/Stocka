using Application.Bases;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Accounting;
using Application.QueryRepositories;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Queries.JournalEntryLine.GetAll
{
    /// <summary>
    /// The brand ID is injected automatically.
    /// </summary>
    public class GetAllJournalEntryLinesQuery : IRequest<Response<IEnumerable<JournalEntryLineDto>>>
    {
    }

    public class GetAllJournalEntryLinesQueryHandler : BaseHandler<IJournalEntryLineQueryRepository>, IRequestHandler<GetAllJournalEntryLinesQuery, Response<IEnumerable<JournalEntryLineDto>>>
    {
        private readonly ICurrentUserContext _currentUser;

        public GetAllJournalEntryLinesQueryHandler(IJournalEntryLineQueryRepository Repository, IMapper mapper, ICurrentUserContext currentUser) : base(mapper, Repository)
        {
            _currentUser = currentUser;
        }

        public async Task<Response<IEnumerable<JournalEntryLineDto>>> Handle(GetAllJournalEntryLinesQuery request, CancellationToken cancellationToken)
        {
            var brandId = _currentUser.ActiveBrandId;

            var items = await _repo.GetAllByBrandIdAsync(brandId);
            var dtos = _mapper.Map<IEnumerable<JournalEntryLineDto>>(items);
            return new Response<IEnumerable<JournalEntryLineDto>>(dtos, "Success");
        }
    }
}
