using Application.Features.Queries.Product.GetAll;
using Application.Features.Queries.ProductCategory.GetAll;
using Application.UseCases.ProductCases;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Models.Products;

namespace MVC.Controllers
{
    [Authorize(Roles = "BrandOwner")]
    public class ProductsController : Controller
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await _mediator.Send(new GetAllProductsQuery());
            return View(products.Data);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new CreateProductModel();
            model.Categories = new Dictionary<Guid, string>();

            var categories = (await _mediator.Send(new GetAllProductCategoriesQuery())).Data?.Select(c => new { c.Id, c.Name });
            if (categories == null) { }

            foreach (var category in categories)
                model.Categories.Add(category.Id, category.Name);

            return View(model);
        }

        [HttpPost]
        public async Task CreateConfirm(RegisterProductCommand command)
        {
            var result = await _mediator.Send(command);
            await Index();
        }


        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            {
                var product = await _mediator.Send(new RetrieveProductCommand(id));
                if (product == null)
                    return NotFound();
                return View(product);
            }
        }


        [HttpGet]
        public async Task<IActionResult> Edit(
            Guid id,
            CancellationToken cancellationToken)
        {
            var product = await _mediator.Send(
                new RetrieveProductCommand(id),
                cancellationToken);

            if (product == null)
                return NotFound();

            return View(product);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            PartialUpdateProductRequest request,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                // لازم نرجع ProductDto هنا أو نعمل ViewModel موحد
                return View();
            }

            var command = new UpdateProductDetailsCommand
            {
                Id = request.Id,
                CategoryId = request.CategoryId,
                Name = request.Name,
                SellingPrice = request.SellingPrice,
                Barcode = request.Barcode,
                IsActive = request.IsActive
            };

            if (request.Image != null && request.Image.Length > 0)
            {
                command.Image = request.Image.OpenReadStream();
                command.ImageExtension =
                    Path.GetExtension(request.Image.FileName);
            }

            await _mediator.Send(command, cancellationToken);

            return RedirectToAction(
                nameof(Details),
                new { id = request.Id });
        }
    }
}