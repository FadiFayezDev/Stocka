using Application.Bases;
using Application.Dtos.Sales;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Commands.Sale.Delete
{
    public class DeleteSaleCommand : IRequest<Response<bool>>
    {
        public Guid Id { get; set; }
    }

    public class DeleteSaleCommandHandler : BaseHandler<ISaleCommandRepository>, IRequestHandler<DeleteSaleCommand, Response<bool>>
    {
        public DeleteSaleCommandHandler(ISaleCommandRepository Repository) : base(null, Repository)
        {
        }

        public async Task<Response<bool>> Handle(DeleteSaleCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null)
                return new Response<bool>("Not found");

            await _repo.DeleteAsync(existing);

            return new Response<bool>();
        }
    }
}
