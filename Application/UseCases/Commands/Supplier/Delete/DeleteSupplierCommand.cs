using Application.Bases;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Purchasing;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Commands.Supplier.Delete
{
    public class DeleteSupplierCommand : IRequest<Response<bool>>
    {
        public Guid Id { get; set; }
    }

    public class DeleteSupplierCommandHandler : BaseHandler<ISupplierCommandRepository>, IRequestHandler<DeleteSupplierCommand, Response<bool>>
    {
        public DeleteSupplierCommandHandler(IMapper mapper,ISupplierCommandRepository repository, IUnitOfWork unitOfWork)
            : base(mapper, repository, unitOfWork)
        {
        }

        public async Task<Response<bool>> Handle(DeleteSupplierCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null)
                throw new BusinessException("Supplier not found");

            return await ExecuteDeleteAsync(
                existing,
                async (s) => await _repo.DeleteAsync(s),
                cancellationToken);
        }
    }
}
