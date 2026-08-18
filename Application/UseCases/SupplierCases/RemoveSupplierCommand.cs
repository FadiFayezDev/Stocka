using Application.Bases;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Purchasing;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.UseCases.SupplierCases
{
    public class RemoveSupplierCommand : IRequest<Response<bool>>
    {
        public Guid Id { get; set; }
    }

    public class RemoveSupplierCommandHandler : BaseHandler<ISupplierCommandRepository>, IRequestHandler<RemoveSupplierCommand, Response<bool>>
    {
        public RemoveSupplierCommandHandler(IMapper mapper,ISupplierCommandRepository repository, IUnitOfWork unitOfWork)
            : base(mapper, repository, unitOfWork)
        {
        }

        public async Task<Response<bool>> Handle(RemoveSupplierCommand request, CancellationToken cancellationToken)
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
