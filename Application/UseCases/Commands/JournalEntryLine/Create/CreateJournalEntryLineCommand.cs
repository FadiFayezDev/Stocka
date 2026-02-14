using Application.Bases;
using Application.Dtos.Accounting;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Commands.JournalEntryLine.Create
{
    public class CreateJournalEntryLineCommand : IRequest<Response<JournalEntryLineDto>>
    {
        public Guid JournalEntryId { get; set; }
        public Guid AccountId { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
    }

    public class CreateJournalEntryLineCommandHandler : BaseHandler<IJournalEntryLineCommandRepository>, IRequestHandler<CreateJournalEntryLineCommand, Response<JournalEntryLineDto>>
    {
        public CreateJournalEntryLineCommandHandler(IJournalEntryLineCommandRepository Repository, IMapper mapper) : base(mapper, Repository)
        {
        }

        public async Task<Response<JournalEntryLineDto>> Handle(CreateJournalEntryLineCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Domain.Entities.Accounting.JournalEntryLine>(request);

            await _repo.CreateAsync(entity);
            return new Response<JournalEntryLineDto>();
        }
    }
}
