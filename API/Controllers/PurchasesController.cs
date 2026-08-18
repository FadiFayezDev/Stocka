using Application.Dtos.Purchasing;
using Application.QueryRepositories;
using Application.UseCases.Purchase;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PurchasesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IPurchaseQueryRepository _purchaseQuery;
        public PurchasesController(IMediator mediator, IPurchaseQueryRepository purchaseQuery)
        {
            _mediator = mediator;
            _purchaseQuery = purchaseQuery;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var purchases = await _purchaseQuery.GetAllWithItemsAsync();
            return Ok(purchases);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var purchase = await _purchaseQuery.GetByIdWithItemsAsync(id);
            if (purchase == null)
                return NotFound();
            return Ok(purchase);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ReceivePurchaseCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateReceivedPurchaseCommand command, CancellationToken cancellationToken)
        {
            command.Id = id;
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpGet("with-items")]
        public async Task<IActionResult> GetAllWithItems(CancellationToken cancellationToken)
        {
            var purchases = await _purchaseQuery.GetAllWithItemsAsync();
            return Ok(purchases);
        }
    }
}