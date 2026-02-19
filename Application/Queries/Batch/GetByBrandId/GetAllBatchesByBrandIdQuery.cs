using Application.Bases;
using Application.Dtos.Products;
using Application.QueryRepositories;
using AutoMapper;
using MediatR;

namespace Application.Queries.Batch.GetByBrandId
{
    public class GetAllBatchesByBrandIdQuery : IRequest<Response<IEnumerable<BatchDto>>>
    {
        public Guid BrandId { get; set; }

        public GetAllBatchesByBrandIdQuery(Guid brandId)
        {
            BrandId = brandId;
        }
    }

    public class GetAllBatchesByBrandIdQueryHandler : IRequestHandler<GetAllBatchesByBrandIdQuery, Response<IEnumerable<BatchDto>>>
    {
        private readonly IBatchQueryRepository _repository;
        private readonly IMapper _mapper;

        public GetAllBatchesByBrandIdQueryHandler(IBatchQueryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<BatchDto>>> Handle(GetAllBatchesByBrandIdQuery request, CancellationToken cancellationToken)
        {
            var batches = await _repository.GetAllByBrandIdAsync(request.BrandId);
            if (batches == null)
                return new Response<IEnumerable<BatchDto>>("Batches not found");

            var batchDtos = _mapper.Map<IEnumerable<BatchDto>>(batches);
            return new Response<IEnumerable<BatchDto>>(batchDtos, "Success");
        }
    }
}
