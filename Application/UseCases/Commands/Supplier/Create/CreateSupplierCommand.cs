using Application.Bases;
using Application.Common.Interfaces;
using Application.Dtos.Purchasing;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Commands.Supplier.Create
{
    public class CreateSupplierCommand : IRequest<Response<SupplierDto>>
    {
        public Guid BrandId { get; set; }
        public string Name { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
    }

    public class CreateSupplierCommandHandler : BaseHandler<ISupplierCommandRepository>, IRequestHandler<CreateSupplierCommand, Response<SupplierDto>>
    {
        public CreateSupplierCommandHandler(ISupplierCommandRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
            : base(mapper, repository, unitOfWork)
        {
        }

        public async Task<Response<SupplierDto>> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Domain.Entities.Purchasing.Supplier>(request);

            return await ExecuteCreateAsync<Domain.Entities.Purchasing.Supplier, SupplierDto>(
                entity,
                async (s) => await _repo.CreateAsync(s),
                cancellationToken);
        }
    }
}
