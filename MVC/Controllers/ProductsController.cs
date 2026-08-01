using Application.Features.Queries.Product.GetAll;
using Application.Features.Queries.ProductCategory.GetAll;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MVC.Models.Products;

namespace MVC.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator) {
            _mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _mediator.Send(new GetAllProductsQuery());
            return View(products.Data);
        }

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
    }
}
