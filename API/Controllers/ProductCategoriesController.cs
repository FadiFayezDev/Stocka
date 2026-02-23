using API.Controllers.Base;
using Application.Bases;
using Application.Dtos.Products;
using Application.Features.Commands.ProductCategory.Create;
using Application.Features.Commands.ProductCategory.Delete;
using Application.Features.Commands.ProductCategory.Update;
using Application.Features.Queries.ProductCategory.GetAll;
using Application.Features.Queries.ProductCategory.GetById;
using Application.Queries.ProductCategory.GetByBrandId;
using Application.UseCases.Commands.ProductCategory.PartialUpdate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Authorize(Roles = "BrandOwner")]
    [ApiController]
    [Route("[controller]")]
    public class ProductCategoriesController : BaseController
    {
        public ProductCategoriesController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Retrieves all product categories.
        /// </summary>
        /// <remarks>Requires the caller to have the "Dev" role. Returns a 200 OK response with the list
        /// of product categories on success, or a 500 Internal Server Error if an unexpected error occurs.</remarks>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>An <see cref="IActionResult"/> containing a response with a collection of product categories if successful;
        /// otherwise, a response indicating the error.</returns>
        [Authorize(Roles = "Dev")]
        [HttpGet("all")]
        [ProducesResponseType(typeof(Response<IEnumerable<ProductCategoryDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetAllProductCategoriesQuery(), cancellationToken);
            if (!result.Succeeded)
                return BadRequest(result);
            return Ok(result);
        }

        /// <summary>
        /// Get all product categories
        /// </summary>
        [HttpGet("by-brand/{brandId:guid}")]
        [ProducesResponseType(typeof(Response<IEnumerable<ProductCategoryDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllByBrandId(Guid brandId, CancellationToken cancellationToken = default)
        {
            var d = User.FindAll("brandId").Select(c => c.Value.ToLower()).ToList();
            if (!d.Contains(brandId.ToString().ToLower()))
                return BadRequest("Invaled brand id");

            var result = await _mediator.Send(new GetAllProductCategoriesByBrandIdQuery(brandId), cancellationToken);
            if (!result.Succeeded)
                return BadRequest(result);
            return Ok(result);
        }

        /// <summary>
        /// Get product category by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Response<ProductCategoryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetProductCategoryByIdQuery(id), cancellationToken);
            if (!result.Succeeded)
                return NotFound(result);
            return Ok(result);
        }

        /// <summary>
        /// Create a new product category
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(Response<ProductCategoryDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CreateProductCategoryCommand command, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(command, cancellationToken);
            if (!result.Succeeded)
                return BadRequest(result);
            return CreatedAtAction(nameof(GetById), new { id = result.Data?.Id }, result);
        }

        /// <summary>
        /// Update an existing product category
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Response<ProductCategoryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductCategoryCommand command, CancellationToken cancellationToken = default)
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
        /// Update an existing product category
        /// </summary>
        [HttpPatch("{id:guid}")]
        [ProducesResponseType(typeof(Response<ProductCategoryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PartialUpdate(Guid id, [FromForm] PartialUpdateProductCategoryCommand command, CancellationToken cancellationToken = default)
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
        /// Delete a product category
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new DeleteProductCategoryCommand(id), cancellationToken);
            if (!result.Succeeded)
                return NotFound(result);
            return Ok(result);
        }
    }
}
