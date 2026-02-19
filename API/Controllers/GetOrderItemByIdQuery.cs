using Application.Bases;
using Application.Dtos.Orders;
using MediatR;

namespace API.Controllers
{
    internal class GetOrderItemByIdQuery : IRequest<Response<OrderItemDto>>
    {
        public Guid Id { get; set; }
    }
}