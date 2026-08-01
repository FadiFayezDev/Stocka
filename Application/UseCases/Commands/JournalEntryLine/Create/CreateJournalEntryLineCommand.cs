using Application.Bases;
using Application.Common.Interfaces;
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
        public CreateJournalEntryLineCommandHandler(IJournalEntryLineCommandRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
            : base(mapper, repository, unitOfWork)
        {
        }

        public async Task<Response<JournalEntryLineDto>> Handle(CreateJournalEntryLineCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Domain.Entities.Accounting.JournalEntryLine>(request);

            return await ExecuteCreateAsync<Domain.Entities.Accounting.JournalEntryLine, JournalEntryLineDto>(
                entity,
                async (jel) => await _repo.CreateAsync(jel),
                cancellationToken);
        }
    }
}
