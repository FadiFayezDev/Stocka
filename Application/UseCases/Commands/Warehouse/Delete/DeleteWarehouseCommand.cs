using Application.Bases;
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
        public DeleteWarehouseCommandHandler(IWarehouseCommandRepository Repository) : base(null, Repository)
        {
        }

        public async Task<Response<bool>> Handle(DeleteWarehouseCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null)
                return new Response<bool>("Not found");

            await _repo.DeleteAsync(existing);


            return new Response<bool>();
        }
    }
}
