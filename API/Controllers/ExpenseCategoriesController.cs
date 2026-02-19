using API.Controllers.Base;
using Application.Bases;
using Application.Dtos.Expenses;
using Application.Features.Commands.ExpenseCategory.Create;
using Application.Features.Commands.ExpenseCategory.Delete;
using Application.Features.Commands.ExpenseCategory.Update;
using Application.Features.Queries.ExpenseCategory.GetAll;
using Application.Features.Queries.ExpenseCategory.GetById;
using Application.Queries.ExpenseCategory.GetByBrandId;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExpenseCategoriesController : BaseController
    {
        public ExpenseCategoriesController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Get all expense categories
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(Response<IEnumerable<ExpenseCategoryDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetAllExpenseCategoriesQuery(), cancellationToken);
            if (!result.Succeeded)
                return BadRequest(result);
            return Ok(result);
        }

        /// <summary>
        /// Get all expense categories by brand ID
        /// </summary>
        [HttpGet("by-brand/{brandId:guid}")]
        [ProducesResponseType(typeof(Response<IEnumerable<ExpenseCategoryDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllByBrandId(Guid brandId, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetAllExpenseCategoriesByBrandIdQuery(brandId), cancellationToken);
            if (!result.Succeeded)
                return BadRequest(result);
            return Ok(result);
        }

        /// <summary>
        /// Get expense category by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Response<ExpenseCategoryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetExpenseCategoryByIdQuery { Id = id }, cancellationToken);
            if (!result.Succeeded)
                return NotFound(result);
            return Ok(result);
        }

        /// <summary>
        /// Create a new expense category
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(Response<ExpenseCategoryDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CreateExpenseCategoryCommand command, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(command, cancellationToken);
            if (!result.Succeeded)
                return BadRequest(result);
            return CreatedAtAction(nameof(GetById), new { id = result.Data?.Id }, result);
        }

        /// <summary>
        /// Update an existing expense category
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Response<ExpenseCategoryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateExpenseCategoryCommand command, CancellationToken cancellationToken = default)
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
        /// Delete an expense category
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new DeleteExpenseCategoryCommand { Id = id }, cancellationToken);
            if (!result.Succeeded)
                return NotFound(result);
            return Ok(result);
        }
    }
}
