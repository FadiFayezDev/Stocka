using Application.Bases;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Products;
using Application.QueryRepositories;
using AutoMapper;
using MediatR;

namespace Application.Queries.Batch.GetByBrandId
{
    public class GetAllBatchesByBrandIdQuery : IRequest<Response<IEnumerable<BatchDto>>>
    {
    }

    public class GetAllBatchesByBrandIdQueryHandler : IRequestHandler<GetAllBatchesByBrandIdQuery, Response<IEnumerable<BatchDto>>>
    {
        private readonly IBatchQueryRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserContext _currentUser;

        public GetAllBatchesByBrandIdQueryHandler(IBatchQueryRepository repository, IMapper mapper, ICurrentUserContext currentUser)
        {
            _repository = repository;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<Response<IEnumerable<BatchDto>>> Handle(GetAllBatchesByBrandIdQuery request, CancellationToken cancellationToken)
        {
            var brandId = _currentUser.ActiveBrandId;
            
            var batches = await _repository.GetAllByBrandIdAsync(brandId);
            if (batches == null)
                return new Response<IEnumerable<BatchDto>>("Batches not found");

            var batchDtos = _mapper.Map<IEnumerable<BatchDto>>(batches);
            return new Response<IEnumerable<BatchDto>>(batchDtos, "Success");
        }
    }
}
