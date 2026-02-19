using Application.Bases;
using Application.Common.Interfaces;
using Application.Dtos.Products;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Commands.WarehouseBatch.Delete
{
    public class DeleteWarehouseBatchCommand : IRequest<Response<bool>>
    {
        public Guid Id { get; set; }
    }

    public class DeleteWarehouseBatchCommandHandler : BaseHandler<IWarehouseBatchCommandRepository>, IRequestHandler<DeleteWarehouseBatchCommand, Response<bool>>
    {
        public DeleteWarehouseBatchCommandHandler(IWarehouseBatchCommandRepository repository, IUnitOfWork unitOfWork)
            : base(null, repository, unitOfWork)
        {
        }

        public async Task<Response<bool>> Handle(DeleteWarehouseBatchCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null)
                return new Response<bool>(false, "Warehouse batch not found");

            return await ExecuteDeleteAsync(
                existing,
                async (wb) => await _repo.DeleteAsync(wb),
                cancellationToken);
        }
    }
}
