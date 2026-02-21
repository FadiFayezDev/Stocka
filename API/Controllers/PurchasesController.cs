using API.Controllers.Base;
using Application.Bases;
using Application.Dtos.Purchasing;
using Application.Features.Commands.Purchase.Create;
using Application.Features.Commands.Purchase.Delete;
using Application.Features.Commands.Purchase.Update;
using Application.Features.Queries.Purchase.GetAll;
using Application.Features.Queries.Purchase.GetById;
using Application.Queries.Purchase.GetByBrandId;
using Application.UseCases;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PurchasesController : BaseController
    {
        public PurchasesController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Get all purchases
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(Response<IEnumerable<PurchaseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetAllPurchasesQuery(), cancellationToken);
            if (!result.Succeeded)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpGet("by-brand/{brandId:guid}")]
        [ProducesResponseType(typeof(Response<IEnumerable<PurchaseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllByBrandId(Guid brandId, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetAllPurchasesByBrandIdQuery(brandId), cancellationToken);
            if (!result.Succeeded)
                return BadRequest(result);
            return Ok(result);
        }

        /// <summary>
        /// Get purchase by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Response<PurchaseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetPurchaseByIdQuery { Id = id }, cancellationToken);
            if (!result.Succeeded)
                return NotFound(result);
            return Ok(result);
        }

        /// <summary>
        /// Create a new purchase
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(Response<PurchaseDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CreatePurchaseCommand command, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(command, cancellationToken);
            if (!result.Succeeded)
                return BadRequest(result);
            return CreatedAtAction(nameof(GetById), new { id = result.Data?.Id }, result);
        }

        /// <summary>
        /// Update an existing purchase
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Response<PurchaseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePurchaseCommand command, CancellationToken cancellationToken = default)
        {
            if (id != command.Id)
                return BadRequest("ID mismatch");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(command, cancellationToken);
            if (!result.Succeeded)
                return NotFound(result);
            return Ok(result);
        }

        /// <summary>
        /// Delete a purchase
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new DeletePurchaseCommand { Id = id }, cancellationToken);
            if (!result.Succeeded)
                return NotFound(result);
            return Ok(result);
        }

        [HttpPost("receive")]
        public async Task<IActionResult> ReceivePurchasedProducts(ReceiveAndStockPurchasedProductsCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}