using Application.Bases;
using Domain.Repositories.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Commands.Batch.Delete
{
    public class DeleteBatchCommand : IRequest<Response<bool>>
    {
        public Guid Id { get; set; }
    }

    public class DeleteBatchCommandHandler : BaseHandler<IBatchCommandRepository>, IRequestHandler<DeleteBatchCommand, Response<bool>>
    {
        public DeleteBatchCommandHandler(IBatchCommandRepository Repository) : base(null, Repository)
        {
        }

        public async Task<Response<bool>> Handle(DeleteBatchCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null)
                return new Response<bool>("Not found");

            await _repo.DeleteAsync(existing);


            return new Response<bool>();
        }
    }
}