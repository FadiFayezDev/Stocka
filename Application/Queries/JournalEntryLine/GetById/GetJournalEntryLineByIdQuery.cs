using Application.Bases;
using Application.Dtos.Accounting;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Queries.JournalEntryLine.GetById
{
    public class GetJournalEntryLineByIdQuery : IRequest<Response<JournalEntryLineDto>>
    {
        public Guid Id { get; set; }
    }

    public class GetJournalEntryLineByIdQueryHandler : BaseHandler<IJournalEntryLineCommandRepository>, IRequestHandler<GetJournalEntryLineByIdQuery, Response<JournalEntryLineDto>>
    {
        public GetJournalEntryLineByIdQueryHandler(IJournalEntryLineCommandRepository Repository, IMapper mapper) : base(mapper, Repository)
        {
        }

        public async Task<Response<JournalEntryLineDto>> Handle(GetJournalEntryLineByIdQuery request, CancellationToken cancellationToken)
        {
            var item = await _repo.GetByIdAsync(request.Id);
            if (item == null)
                return new Response<JournalEntryLineDto>("Not found");

            var dto = _mapper.Map<JournalEntryLineDto>(item);
            return new Response<JournalEntryLineDto>(dto, "Success");
        }
    }
}
