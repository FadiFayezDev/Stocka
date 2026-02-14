using Application.Bases;
using Application.Dtos.Products;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Queries.Batch.GetById
{
    public class GetBatchByIdQuery : IRequest<Response<BatchDto>>
    {
        public Guid Id { get; set; }
    }

    public class GetBatchByIdQueryHandler : BaseHandler<IBatchCommandRepository>, IRequestHandler<GetBatchByIdQuery, Response<BatchDto>>
    {
        public GetBatchByIdQueryHandler(IBatchCommandRepository Repository, IMapper mapper) : base(mapper, Repository)
        {
        }

        public async Task<Response<BatchDto>> Handle(GetBatchByIdQuery request, CancellationToken cancellationToken)
        {
            var item = await _repo.GetByIdAsync(request.Id);
            if (item == null)
                return new Response<BatchDto>("Not found");

            var dto = _mapper.Map<BatchDto>(item);
            return new Response<BatchDto>(dto, "Success");
        }
    }
}
