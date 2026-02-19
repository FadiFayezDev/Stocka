using Application.Bases;
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
        public Guid BrandId { get; set; }
        public string Name { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
    }

    public class UpdateSupplierCommandHandler : BaseHandler<ISupplierCommandRepository>, IRequestHandler<UpdateSupplierCommand, Response<SupplierDto>>
    {
        public UpdateSupplierCommandHandler(ISupplierCommandRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
            : base(mapper, repository, unitOfWork)
        {
        }

        public async Task<Response<SupplierDto>> Handle(UpdateSupplierCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null)
                return new Response<SupplierDto>("Supplier not found");

            _mapper.Map(request, existing);

            return await ExecuteUpdateAsync<Domain.Entities.Purchasing.Supplier, SupplierDto>(
                existing,
                async (s) => await _repo.UpdateAsync(s),
                cancellationToken);
        }
    }
}
