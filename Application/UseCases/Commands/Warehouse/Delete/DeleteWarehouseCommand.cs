using Application.Bases;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Products;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Commands.Warehouse.Delete
{
    public class DeleteWarehouseCommand : IRequest<Response<bool>>
    {
        public Guid Id { get; set; }
    }

    public class DeleteWarehouseCommandHandler : BaseHandler<IWarehouseCommandRepository>, IRequestHandler<DeleteWarehouseCommand, Response<bool>>
    {
        public DeleteWarehouseCommandHandler(IMapper mapper,IWarehouseCommandRepository repository, IUnitOfWork unitOfWork)
            : base(mapper, repository, unitOfWork)
        {
        }

        public async Task<Response<bool>> Handle(DeleteWarehouseCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null)
                throw new BusinessException("Warehouse not found");

            return await ExecuteDeleteAsync(
                existing,
                async (w) => await _repo.DeleteAsync(w),
                cancellationToken);
        }
    }
}
