using Application.Bases;
using Application.Dtos.Core;
using AutoMapper;
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
        public DeleteBrandCommandHandler(IBrandCommandRepository brandRepository) : base(null, brandRepository)
        {
        }

        public async Task<Response<bool>> Handle(DeleteBrandCommand request, CancellationToken cancellationToken)
        {
            var existingBrand = await _repo.GetByIdAsync(request.Id);
            if (existingBrand == null)
                return new Response<bool>("Brand not found");

            await _repo.DeleteAsync(existingBrand);

            return new Response<bool>();
        }
    }
}
