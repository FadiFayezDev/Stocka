using Application.Bases;
using Application.Common.Interfaces;
using Application.Dtos.Products;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Commands.WarehouseBatch.Update
{
    public class UpdateWarehouseBatchCommand : IRequest<Response<WarehouseBatchDto>>
    {
        public Guid Id { get; set; }
        public Guid WarehouseId { get; set; }
        public Guid BatchId { get; set; }
        public int Quantity { get; set; }
    }

    public class UpdateWarehouseBatchCommandHandler : BaseHandler<IWarehouseBatchCommandRepository>, IRequestHandler<UpdateWarehouseBatchCommand, Response<WarehouseBatchDto>>
    {
        public UpdateWarehouseBatchCommandHandler(IWarehouseBatchCommandRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
            : base(mapper, repository, unitOfWork)
        {
        }

        public async Task<Response<WarehouseBatchDto>> Handle(UpdateWarehouseBatchCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null)
                return new Response<WarehouseBatchDto>("Warehouse batch not found");

            _mapper.Map(request, existing);

            return await ExecuteUpdateAsync<Domain.Entities.Products.WarehouseBatch, WarehouseBatchDto>(
                existing,
                async (wb) => await _repo.UpdateAsync(wb),
                cancellationToken);
        }
    }
}
