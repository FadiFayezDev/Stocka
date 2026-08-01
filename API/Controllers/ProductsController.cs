using API.Models;
using Application.UseCases.ProductCases;
using Application.UseCases.Category;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> ListProductsAsync(CancellationToken cancellationToken)
        {
            return Ok(await _mediator.Send(new ListProductsCommand(), cancellationToken));
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> ListProductsAsync(Guid id, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _mediator.Send(new RetrieveProductCommand(id), cancellationToken));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] CreateProductRequest request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var command = new CreateProductCommand
            {
                CategoryId = request.CategoryId,
                Name = request.Name,
                SellingPrice = request.SellingPrice,
                Barcode = request.Barcode,
            };

            if (request.Image != null)
            {
                command.Image = request.Image.OpenReadStream();
                command.ImageExtension = Path.GetExtension(request.Image.FileName);
            }

            return Ok(await _mediator.Send(command, cancellationToken));
        }

        [HttpPatch]
        public async Task<IActionResult> PartialUpdateAsync([FromForm] PartialUpdateProductRequest request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var command = new PartialUpdateProductCommand
            {
                Id = request.Id,
                CategoryId = request.CategoryId,
                Name = request.Name,
                SellingPrice = request.SellingPrice,
                Barcode = request.Barcode,
                IsActive = request.IsActive,
            };

            if (request.Image != null)
            {
                command.Image = request.Image.OpenReadStream();
                command.ImageExtension = Path.GetExtension(request.Image.FileName);
            }

            return Ok(await _mediator.Send(command, cancellationToken));
        }

        [HttpGet("categories")]
        public async Task<IActionResult> ListCategoriesAsync(CancellationToken cancellationToken)
        {
            return Ok(await _mediator.Send(new ListCategoriesCommand(), cancellationToken));
        }

        [HttpPost("categories")]
        public async Task<IActionResult> CreateCategoryAsync([FromBody] CreateCategoryCommand command, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _mediator.Send(command, cancellationToken));
        }

        [HttpPatch("assign-category")]
        public async Task<IActionResult> AssignCategoryAsync([FromBody] AssignCategoryCommand command, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _mediator.Send(command, cancellationToken));
        }
    }
}