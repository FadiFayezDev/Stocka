using Application.Bases;
using Application.Dtos.Products;
using Application.QueryRepositories;
using AutoMapper;
using MediatR;

namespace Application.Queries.Batch.GetAll
{
    public class GetAllBatchesQuery : IRequest<Response<IEnumerable<BatchDto>>>
    {
    }

    public class GetAllBatchesQueryHandler : IRequestHandler<GetAllBatchesQuery, Response<IEnumerable<BatchDto>>>
    {
        private readonly IBatchQueryRepository _queryRepository;
        private readonly IMapper _mapper;

        public GetAllBatchesQueryHandler(IBatchQueryRepository queryRepository, IMapper mapper)
        {
            _queryRepository = queryRepository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<BatchDto>>> Handle(GetAllBatchesQuery request, CancellationToken cancellationToken)
        {
            var batches = await _queryRepository.GetAllTableAsync();
            var batchDtos = _mapper.Map<IEnumerable<BatchDto>>(batches);
            return new Response<IEnumerable<BatchDto>>(batchDtos, "Success");
        }
    }
}