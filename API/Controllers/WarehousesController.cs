using Application.UseCases.WarehouseCases;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WarehousesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WarehousesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateWarehouseAsync([FromBody] RegisterWarehouseCommand command, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _mediator.Send(command, cancellationToken));
        }

        [HttpGet]
        public async Task<IActionResult> ListWarehousesAsync(CancellationToken cancellationToken)
        {
            return Ok(await _mediator.Send(new ListWarehousesCommand(), cancellationToken));
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> RetrieveWarehouseAsync(Guid id, CancellationToken cancellationToken)
        {
            return Ok(await _mediator.Send(new RetrieveWarehouseCommand(id), cancellationToken));
        }

        [HttpPatch]
        public async Task<IActionResult> PartialUpdateWarehouseAsync([FromBody] UpdateWarehouseInformationCommand command, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _mediator.Send(command, cancellationToken));
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> TransferStockAsync([FromBody] TransferStockCommand command, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _mediator.Send(command, cancellationToken));
        }
    }
}
