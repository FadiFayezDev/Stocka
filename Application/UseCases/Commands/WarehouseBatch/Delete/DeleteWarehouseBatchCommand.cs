using Application.Bases;
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
        public DeleteWarehouseBatchCommandHandler(IWarehouseBatchCommandRepository Repository) : base(null, Repository)
        {
        }

        public async Task<Response<bool>> Handle(DeleteWarehouseBatchCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null)
                return new Response<bool>("Not found");

            await _repo.DeleteAsync(existing);


            return new Response<bool>();
        }
    }
}
