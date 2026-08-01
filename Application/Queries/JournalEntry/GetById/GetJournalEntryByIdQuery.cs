using Application.Bases;
using Application.Dtos.Accounting;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Queries.JournalEntry.GetById
{
    public class GetJournalEntryByIdQuery : IRequest<Response<JournalEntryDto>>
    {
        public Guid Id { get; set; }
    }

    public class GetJournalEntryByIdQueryHandler : BaseHandler<IJournalEntryCommandRepository>, IRequestHandler<GetJournalEntryByIdQuery, Response<JournalEntryDto>>
    {
        public GetJournalEntryByIdQueryHandler(IJournalEntryCommandRepository Repository, IMapper mapper) : base(mapper, Repository)
        {
        }

        public async Task<Response<JournalEntryDto>> Handle(GetJournalEntryByIdQuery request, CancellationToken cancellationToken)
        {
            var item = await _repo.GetByIdAsync(request.Id);
            if (item == null)
                return new Response<JournalEntryDto>("Not found");

            var dto = _mapper.Map<JournalEntryDto>(item);
            return new Response<JournalEntryDto>(dto, "Success");
        }
    }
}
