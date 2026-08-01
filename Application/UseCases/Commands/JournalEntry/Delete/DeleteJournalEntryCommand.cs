using Application.Bases;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Accounting;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Commands.JournalEntry.Delete
{
    public class DeleteJournalEntryCommand : IRequest<Response<bool>>
    {
        public Guid Id { get; set; }
    }

    public class DeleteJournalEntryCommandHandler : BaseHandler<IJournalEntryCommandRepository>, IRequestHandler<DeleteJournalEntryCommand, Response<bool>>
    {
        public DeleteJournalEntryCommandHandler(IMapper mapper, IJournalEntryCommandRepository repository, IUnitOfWork unitOfWork)
            : base(mapper, repository, unitOfWork)
        {
        }

        public async Task<Response<bool>> Handle(DeleteJournalEntryCommand request, CancellationToken cancellationToken)
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
