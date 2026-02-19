using Application.Bases;
using Application.Common.Interfaces;
using Application.Dtos.Accounting;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Commands.JournalEntryLine.Update
{
    public class UpdateJournalEntryLineCommand : IRequest<Response<JournalEntryLineDto>>
    {
        public Guid Id { get; set; }
        public Guid JournalEntryId { get; set; }
        public Guid AccountId { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
    }

    public class UpdateJournalEntryLineCommandHandler : BaseHandler<IJournalEntryLineCommandRepository>, IRequestHandler<UpdateJournalEntryLineCommand, Response<JournalEntryLineDto>>
    {
        public UpdateJournalEntryLineCommandHandler(IJournalEntryLineCommandRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
            : base(mapper, repository, unitOfWork)
        {
        }

        public async Task<Response<JournalEntryLineDto>> Handle(UpdateJournalEntryLineCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null)
                return new Response<JournalEntryLineDto>("Journal entry line not found");

            _mapper.Map(request, existing);

            return await ExecuteUpdateAsync<Domain.Entities.Accounting.JournalEntryLine, JournalEntryLineDto>(
                existing,
                async (jel) => await _repo.UpdateAsync(jel),
                cancellationToken);
        }
    }
}
