using Application.Bases;
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
        public CreateSupplierCommandHandler(ISupplierCommandRepository Repository, IMapper mapper) : base(mapper, Repository)
        {
        }

        public async Task<Response<SupplierDto>> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Domain.Entities.Purchasing.Supplier>(request);

            await _repo.CreateAsync(entity);

            return new Response<SupplierDto>();
        }
    }
}
