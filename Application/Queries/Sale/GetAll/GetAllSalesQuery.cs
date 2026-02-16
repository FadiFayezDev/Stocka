using Application.Bases;
using Application.Dtos.Orders;
using Application.QueryRepositories;
using AutoMapper;
using MediatR;

namespace Application.Queries.Order.GetAll
{
    public class GetAllOrdersQuery : IRequest<Response<IEnumerable<OrderDto>>>
    {
    }

    public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, Response<IEnumerable<OrderDto>>>
    {
        private readonly IOrderQueryRepository _repository;
        private readonly IMapper _mapper;

        public GetAllOrdersQueryHandler(IOrderQueryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<OrderDto>>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            var items = await _repository.GetAllTableAsync();
            var dtos = _mapper.Map<IEnumerable<OrderDto>>(items);
            return new Response<IEnumerable<OrderDto>>(dtos, "Success");
        }
    }
}
