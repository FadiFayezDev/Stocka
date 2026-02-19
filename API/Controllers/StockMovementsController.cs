using API.Controllers.Base;
using Application.Bases;
using Application.Dtos.Products;
using Application.Features.Commands.StockMovement.Create;
using Application.Features.Commands.StockMovement.Delete;
using Application.Features.Commands.StockMovement.Update;
using Application.Features.Queries.StockMovement.GetAll;
using Application.Features.Queries.StockMovement.GetById;
using Application.Queries.StockMovement.GetByBrandId;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StockMovementsController : BaseController
    {
        public StockMovementsController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Get all stock movements
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(Response<IEnumerable<StockMovementDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetAllStockMovementsQuery(), cancellationToken);
            if (!result.Succeeded)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpGet("by-brand/{brandId:guid}")]
        [ProducesResponseType(typeof(Response<IEnumerable<StockMovementDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllByBrandId(Guid brandId, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetAllStockMovementsByBrandIdQuery(brandId), cancellationToken);
            if (!result.Succeeded)
                return BadRequest(result);
            return Ok(result);
        }

        /// <summary>
        /// Get stock movement by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Response<StockMovementDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetStockMovementByIdQuery { Id = id }, cancellationToken);
            if (!result.Succeeded)
                return NotFound(result);
            return Ok(result);
        }

        /// <summary>
        /// Create a new stock movement
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(Response<StockMovementDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CreateStockMovementCommand command, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(command, cancellationToken);
            if (!result.Succeeded)
                return BadRequest(result);
            return CreatedAtAction(nameof(GetById), new { id = result.Data?.Id }, result);
        }

        /// <summary>
        /// Update an existing stock movement
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Response<StockMovementDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateStockMovementCommand command, CancellationToken cancellationToken = default)
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
        /// Delete a stock movement
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new DeleteStockMovementCommand { Id = id }, cancellationToken);
            if (!result.Succeeded)
                return NotFound(result);
            return Ok(result);
        }
    }
}
