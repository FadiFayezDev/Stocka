using Application.Bases;
using Domain.Entities.Orders;
using MediatR;

namespace API.Controllers
{
    public class GetAllOrderItemsQuery : IRequest<Response<OrderItem>?>
    {
    }
}