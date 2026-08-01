using Application.Bases;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Products;
using AutoMapper;
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
        public DeleteWarehouseBatchCommandHandler(IMapper mapper, IWarehouseBatchCommandRepository repository, IUnitOfWork unitOfWork)
            : base(mapper, repository, unitOfWork)
        {
        }

        public async Task<Response<bool>> Handle(DeleteWarehouseBatchCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null)
                throw new BusinessException("Warehouse batch not found");

            return await ExecuteDeleteAsync(
                existing,
                async (wb) => await _repo.DeleteAsync(wb),
                cancellationToken);
        }
    }
}