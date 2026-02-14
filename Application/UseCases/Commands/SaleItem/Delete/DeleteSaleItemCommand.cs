using Application.Bases;
using Application.Dtos.Sales;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Commands.SaleItem.Delete
{
    public class DeleteSaleItemCommand : IRequest<Response<bool>>
    {
        public Guid Id { get; set; }
    }

    public class DeleteSaleItemCommandHandler : BaseHandler<ISaleItemCommandRepository>, IRequestHandler<DeleteSaleItemCommand, Response<bool>>
    {
        public DeleteSaleItemCommandHandler(ISaleItemCommandRepository Repository) : base(null, Repository)
        {
        }

        public async Task<Response<bool>> Handle(DeleteSaleItemCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null)
                return new Response<bool>("Not found");

            await _repo.DeleteAsync(existing);

            return new Response<bool>();
        }
    }
}
