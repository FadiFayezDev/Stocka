using Application.Bases;
using Application.Common.Interfaces;
using Application.Dtos.Core;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Commands.Brand.Delete
{
    public class DeleteBrandCommand : IRequest<Response<bool>>
    {
        public Guid Id { get; set; }
    }

    public class DeleteBrandCommandHandler : BaseHandler<IBrandCommandRepository>, IRequestHandler<DeleteBrandCommand, Response<bool>>
    {
        public DeleteBrandCommandHandler(IBrandCommandRepository brandRepository, IUnitOfWork unitOfWork) 
            : base(null, brandRepository, unitOfWork)
        {
        }

        public async Task<Response<bool>> Handle(DeleteBrandCommand request, CancellationToken cancellationToken)
        {
            var existingBrand = await _repo.GetByIdAsync(request.Id);
            if (existingBrand == null)
                return new Response<bool>(false, "Brand not found");

            return await ExecuteDeleteAsync(
                existingBrand,
                async (b) => await _repo.DeleteAsync(b),
                cancellationToken);
        }
    }
}
