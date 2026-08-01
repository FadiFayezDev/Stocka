using Application.Dtos.Orders;
using Application.QueryRepositories;
using Application.UseCases.Commands.Sale.Create;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IOrderQueryRepository _orderQuery;
        public OrdersController(IMediator mediator, IOrderQueryRepository orderQuery)
        {
            _mediator = mediator;
            _orderQuery = orderQuery;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var orders = await _orderQuery.GetAllWithItemsAsync();
            return Ok(orders);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var order = await _orderQuery.GetByIdWithItemsAsync(id);
            if (order == null)
                return NotFound();
            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSaleCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpGet("with-items")]
        public async Task<IActionResult> GetAllWithItems(CancellationToken cancellationToken)
        {
            var orders = await _orderQuery.GetAllWithItemsAsync();
            return Ok(orders);
        }
    }
}