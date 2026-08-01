using Application.QueryRepositories;
using Application.Features.Commands.Supplier.Create;
using Application.Features.Commands.Supplier.Update;
using Application.Features.Commands.Supplier.Delete;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ISupplierQueryRepository _supplierQuery;
        public SuppliersController(IMediator mediator, ISupplierQueryRepository supplierQuery)
        {
            _mediator = mediator;
            _supplierQuery = supplierQuery;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var suppliers = await _supplierQuery.GetAllByBrandIdAsync(Guid.Empty);
            return Ok(suppliers);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var supplier = await _supplierQuery.GetByIdAsync(id);
            if (supplier == null)
                return NotFound();
            return Ok(supplier);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSupplierCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSupplierCommand command, CancellationToken cancellationToken)
        {
            command.Id = id;
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DeleteSupplierCommand { Id = id }, cancellationToken);
            return Ok(result);
        }
    }
}