using Application.QueryRepositories;
using Application.UseCases.ExpenseCases;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IExpenseQueryRepository _expenseQuery;
        public ExpensesController(IMediator mediator, IExpenseQueryRepository expenseQuery)
        {
            _mediator = mediator;
            _expenseQuery = expenseQuery;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var expenses = await _expenseQuery.GetAllByBrandIdAsync(Guid.Empty);
            return Ok(expenses);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var expense = await _expenseQuery.GetByIdAsync(id);
            if (expense == null)
                return NotFound();
            return Ok(expense);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RecordExpenseCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateExpenseCommand command, CancellationToken cancellationToken)
        {
            command.Id = id;
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new VoidExpenseCommand { Id = id }, cancellationToken);
            return Ok(result);
        }
    }
}