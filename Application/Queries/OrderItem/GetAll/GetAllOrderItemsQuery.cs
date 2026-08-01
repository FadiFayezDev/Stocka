using Application.Bases;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Orders;
using Application.QueryRepositories;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Queries.OrderItem.GetAll
{
    /// <summary>
    /// The brand ID is injected automatically.
    /// </summary>
    public class GetAllOrderItemsQuery : IRequest<Response<IEnumerable<OrderItemDto>>>
    {
    }

    public class GetAllOrderItemsQueryHandler : BaseHandler<IOrderItemQueryRepository>, IRequestHandler<GetAllOrderItemsQuery, Response<IEnumerable<OrderItemDto>>>
    {
        private readonly ICurrentUserContext _currentUser;
        public GetAllOrderItemsQueryHandler(IOrderItemQueryRepository Repository, IMapper mapper, ICurrentUserContext currentUser) : base(mapper, Repository)
        {
            _currentUser = currentUser;

        }

        public async Task<Response<IEnumerable<OrderItemDto>>> Handle(GetAllOrderItemsQuery request, CancellationToken cancellationToken)
        {
            var brandId = _currentUser.ActiveBrandId;

            var items = await _repo.GetAllByBrandIdAsync(brandId);
            var dtos = _mapper.Map<IEnumerable<OrderItemDto>>(items);
            return new Response<IEnumerable<OrderItemDto>>(dtos, "Success");
        }
    }
}
