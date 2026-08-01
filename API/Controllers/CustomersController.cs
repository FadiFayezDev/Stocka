using Application.QueryRepositories;
using Application.UseCases.Commands.Customer.Create;
using Application.UseCases.Commands.Customer.Update;
using Application.UseCases.Commands.Customer.Delete;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICustomerQueryRepository _customerQuery;
        public CustomersController(IMediator mediator, ICustomerQueryRepository customerQuery)
        {
            _mediator = mediator;
            _customerQuery = customerQuery;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var customers = await _customerQuery.GetAllByBrandIdAsync(Guid.Empty);
            return Ok(customers);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var customer = await _customerQuery.GetByIdAsync(id);
            if (customer == null)
                return NotFound();
            return Ok(customer);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCustomerCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCustomerCommand command, CancellationToken cancellationToken)
        {
            command.Id = id;
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DeleteCustomerCommand { Id = id }, cancellationToken);
            return Ok(result);
        }
    }
}