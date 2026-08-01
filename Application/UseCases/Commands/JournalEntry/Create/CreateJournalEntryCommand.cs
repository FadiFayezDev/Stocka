using Application.Bases;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Accounting;
using AutoMapper;
using Domain.Entities.Accounting;
using Domain.Primitives;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Commands.JournalEntry.Create
{
    public class CreateJournalEntryCommand : IRequest<Response<JournalEntryDto>>
    {
        public DateTime EntryDate { get; set; }
        public string? Description { get; set; }
    }

    public class CreateJournalEntryCommandHandler : BaseHandler<IJournalEntryCommandRepository>, IRequestHandler<CreateJournalEntryCommand, Response<JournalEntryDto>>
    {
        private readonly ICurrentUserContext _currentUser;
        public CreateJournalEntryCommandHandler(IJournalEntryCommandRepository repository, IMapper mapper, IUnitOfWork unitOfWork, ICurrentUserContext  currentUser)
            : base(mapper, repository, unitOfWork)
        {
            _currentUser = currentUser;
        }

        public async Task<Response<JournalEntryDto>> Handle(CreateJournalEntryCommand request, CancellationToken cancellationToken)
        {
            var brandId = _currentUser.ActiveBrandId;

            var entity = new Domain.Entities.Accounting.JournalEntry(new BrandId(brandId), request.EntryDate, request.Description);

            return await ExecuteCreateAsync<Domain.Entities.Accounting.JournalEntry, JournalEntryDto>(
                entity,
                async (je) => await _repo.CreateAsync(je),
                cancellationToken);
        }
    }
}