using API.Controllers.Base;
using API.Models;
using Application.Bases;
using Application.Dtos;
using Application.Dtos.Products;
using Application.Features.Commands.Product.Create;
using Application.Features.Commands.Product.Delete;
using Application.Features.Commands.Product.Update;
using Application.Features.Queries.Product.GetAll;
using Application.Features.Queries.Product.GetById;
using Application.Queries.Product.GetByBrandId;
using Application.Queries.Product.GetProductsWithWarehousesQuntity;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : BaseController
    {
        public ProductsController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Get all products
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(Response<IEnumerable<ProductDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetAllProductsQuery(), cancellationToken);
            if (!result.Succeeded)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpGet("by-brand/{brandId:guid}")]
        [ProducesResponseType(typeof(Response<IEnumerable<ProductDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllByBrandId(Guid brandId, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetAllProductsByBrandIdQuery(brandId), cancellationToken);
            if (!result.Succeeded)
                return BadRequest(result);
            return Ok(result);
        }

        [ProducesResponseType(typeof(Response<IEnumerable<ProductsWithWarehouseQuntityDto>>), StatusCodes.Status200OK)]
        [HttpGet("with-quantities/{brandId:guid}")]
        public async Task<IActionResult> GetProductsWithQuantities(Guid brandId, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetProductsWithQuantitiesQuery(brandId), cancellationToken);
            if (!result.Succeeded)
                return BadRequest(result);
            return Ok(result);
        }

        /// <summary>
        /// Get product by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Response<ProductDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetProductByIdQuery { Id = id }, cancellationToken);
            if (!result.Succeeded)
                return NotFound(result);
            return Ok(result);
        }

        /// <summary>
        /// Create a new product
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(Response<ProductDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromForm] CreateProductRequest request, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var command = new CreateProductCommand
            {
                BrandId = request.BrandId,
                CategoryId = request.CategoryId,
                Name = request.Name,
                Barcode = request.Barcode,
            };

            if (request.Image != null)
            {
                command.Image = request.Image.OpenReadStream();
                command.ImageExtension = Path.GetExtension(request.Image.FileName);
            }

            var result = await _mediator.Send(command, cancellationToken);
            if (!result.Succeeded)
                return BadRequest(result);
            return CreatedAtAction(nameof(GetById), new { id = result.Data?.Id }, result);
        }

        /// <summary>
        /// Update an existing product
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Response<ProductDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductCommand command, CancellationToken cancellationToken = default)
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
        /// Delete a product
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new DeleteProductCommand { Id = id }, cancellationToken);
            if (!result.Succeeded)
                return NotFound(result);
            return Ok(result);
        }
    }
}
