using Application.Bases;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Accounting;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.UseCases.JournalEntryCases
{
    public class VoidJournalEntryCommand : IRequest<Response<bool>>
    {
        public Guid Id { get; set; }
    }

    public class VoidJournalEntryCommandHandler : BaseHandler<IJournalEntryCommandRepository>, IRequestHandler<VoidJournalEntryCommand, Response<bool>>
    {
        public VoidJournalEntryCommandHandler(IMapper mapper, IJournalEntryCommandRepository repository, IUnitOfWork unitOfWork)
            : base(mapper, repository, unitOfWork)
        {
        }

        public async Task<Response<bool>> Handle(VoidJournalEntryCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null)
                throw new BusinessException("Journal entry not found");

            return await ExecuteDeleteAsync(
                existing,
                async (je) => await _repo.DeleteAsync(je),
                cancellationToken);
        }
    }
}
