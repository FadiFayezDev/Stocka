using Application.Bases;
using Application.Dtos.Orders;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Queries.Order.GetById
{
    public class GetOrderByIdQuery : IRequest<Response<OrderDto>>
    {
        public Guid Id { get; set; }
    }

    public class GetOrderByIdQueryHandler : BaseHandler<IOrderCommandRepository>, IRequestHandler<GetOrderByIdQuery, Response<OrderDto>>
    {
        public GetOrderByIdQueryHandler(IOrderCommandRepository Repository, IMapper mapper) : base(mapper, Repository)
        {
        }

        public async Task<Response<OrderDto>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var item = await _repo.GetByIdAsync(request.Id);
            if (item == null)
                return new Response<OrderDto>("Not found");

            var dto = _mapper.Map<OrderDto>(item);
            return new Response<OrderDto>(dto, "Success");
        }
    }
}
