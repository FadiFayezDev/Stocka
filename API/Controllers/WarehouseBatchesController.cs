using API.Controllers.Base;
using Application.Bases;
using Application.Dtos.Products;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Features.Queries.WarehouseBatch.GetAll;
using Application.Features.Queries.WarehouseBatch.GetById;
using Application.Features.Commands.WarehouseBatch.Create;
using Application.Features.Commands.WarehouseBatch.Update;
using Application.Features.Commands.WarehouseBatch.Delete;
using Application.Queries.WarehouseBatch.GetByBrandId;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WarehouseBatchesController : BaseController
    {
        public WarehouseBatchesController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Get all warehouse batches
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(Response<IEnumerable<WarehouseBatchDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetAllWarehouseBatchesQuery(), cancellationToken);
            if (!result.Succeeded)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpGet("by-brand/{brandId:guid}")]
        [ProducesResponseType(typeof(Response<IEnumerable<WarehouseBatchDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllByBrandId(Guid brandId, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetAllWarehouseBatchesByBrandIdQuery(brandId), cancellationToken);
            if (!result.Succeeded)
                return BadRequest(result);
            return Ok(result);
        }

        /// <summary>
        /// Get warehouse batch by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Response<WarehouseBatchDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetWarehouseBatchByIdQuery { Id = id }, cancellationToken);
            if (!result.Succeeded)
                return NotFound(result);
            return Ok(result);
        }

        /// <summary>
        /// Create a new warehouse batch
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(Response<WarehouseBatchDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CreateWarehouseBatchCommand command, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(command, cancellationToken);
            if (!result.Succeeded)
                return BadRequest(result);
            return CreatedAtAction(nameof(GetById), new { id = result.Data?.Id }, result);
        }

        /// <summary>
        /// Update an existing warehouse batch
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Response<WarehouseBatchDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateWarehouseBatchCommand command, CancellationToken cancellationToken = default)
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
        /// Delete a warehouse batch
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new DeleteWarehouseBatchCommand { Id = id }, cancellationToken);
            if (!result.Succeeded)
                return NotFound(result);
            return Ok(result);
        }
    }
}
