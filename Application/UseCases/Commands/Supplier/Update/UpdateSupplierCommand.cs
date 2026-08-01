using Application.Bases;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Purchasing;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Commands.Supplier.Update
{
    public class UpdateSupplierCommand : IRequest<Response<SupplierDto>>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
    }

    public class UpdateSupplierCommandHandler : BaseHandler<ISupplierCommandRepository>, IRequestHandler<UpdateSupplierCommand, Response<SupplierDto>>
    {
        private readonly ICurrentUserContext _currentUser;
        public UpdateSupplierCommandHandler(ISupplierCommandRepository repository, IMapper mapper, IUnitOfWork unitOfWork, ICurrentUserContext userContext)
            : base(mapper, repository, unitOfWork)
        {
            _currentUser = userContext;
        }

        public async Task<Response<SupplierDto>> Handle(UpdateSupplierCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null)
                throw new BusinessException("Supplier not found");

            existing.UpdateName(request.Name);
            existing.UpdateContactInfo(request.Phone, request.Email, request.Address);

            return await ExecuteUpdateAsync<Domain.Entities.Purchasing.Supplier, SupplierDto>(
                existing,
                async (s) => await _repo.UpdateAsync(s),
                cancellationToken);
        }
    }
}
