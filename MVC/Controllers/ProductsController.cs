using Application.Common.Exceptions;
using Application.Dtos.Products;
using Application.Features.Queries.Brand.GetById;
using Application.Features.Queries.ProductCategory.GetAll;
using Application.Features.Queries.ProductCategory.GetById;
using Application.UseCases.Category;
using Application.UseCases.ProductCases;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Index(string? search = null, string? status = null)
        {
            var products = await _mediator.Send(new ListProductsCommand());

            if (!string.IsNullOrWhiteSpace(search))
            {
                products = products
                    .Where(p =>
                        p.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                        (p.Barcode != null && p.Barcode.Contains(search, StringComparison.OrdinalIgnoreCase)))
                    .ToList();
            }

            if (string.Equals(status, "active", StringComparison.OrdinalIgnoreCase))
                products = products.Where(p => p.IsActive).ToList();
            else if (string.Equals(status, "inactive", StringComparison.OrdinalIgnoreCase))
                products = products.Where(p => !p.IsActive).ToList();

            ViewData["Search"] = search;
            ViewData["Status"] = status;
            ViewData["Title"] = "المنتجات";

            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new CreateProductModel();
            model.Categories = await GetCategoriesAsync();
            ViewData["Title"] = "إضافة منتج جديد";
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = await GetCategoriesAsync();
                return View(model);
            }

            Stream? imageStream = null;
            string? imageExtension = null;

            if (model.ImageFile is { Length: > 0 })
            {
                imageStream = model.ImageFile.OpenReadStream();
                imageExtension = Path.GetExtension(model.ImageFile.FileName);
            }

            var command = new RegisterProductCommand
            {
                CategoryId = model.CategoryId,
                Name = model.Name,
                SellingPrice = model.SellingPrice,
                Barcode = model.Barcode,
                Image = imageStream,
                ImageExtension = imageExtension
            };

            try
            {
                await _mediator.Send(command);
                TempData["SuccessMessage"] = "تم إضافة المنتج بنجاح.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, FriendlyMessage(ex));
                model.Categories = await GetCategoriesAsync(model.CategoryId);
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            try
            {
                var product = await _mediator.Send(new RetrieveProductCommand(id));

                var category = await _mediator.Send(new GetProductCategoryByIdQuery(product.CategoryId));
                var brand = await _mediator.Send(new GetBrandByIdQuery { Id = product.BrandId });

                ViewData["CategoryName"] =
                    category is { Succeeded: true } ? category.Data?.Name : product.CategoryId.ToString();
                ViewData["BrandName"] =
                    brand is { Succeeded: true } ? brand.Data?.Name : product.BrandId.ToString();

                ViewData["Title"] = product.Name;
                return View(product);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "المنتج المطلوب غير موجود.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                var product = await _mediator.Send(new RetrieveProductCommand(id));

                var model = new EditProductModel
                {
                    Id = product.Id,
                    CategoryId = product.CategoryId,
                    Name = product.Name,
                    SellingPrice = product.SellingPrice,
                    Barcode = product.Barcode,
                    IsActive = product.IsActive,
                    ImageUrl = product.ImageUrl
                };

                model.Categories = await GetCategoriesAsync(product.CategoryId);
                ViewData["Title"] = "تعديل المنتج";
                return View(model);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "المنتج المطلوب غير موجود.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditProductModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = await GetCategoriesAsync(model.CategoryId);
                return View(model);
            }

            var command = new UpdateProductDetailsCommand
            {
                Id = model.Id,
                CategoryId = model.CategoryId,
                Name = model.Name,
                SellingPrice = model.SellingPrice,
                Barcode = model.Barcode,
                IsActive = model.IsActive
            };

            if (model.Image is { Length: > 0 })
            {
                command.Image = model.Image.OpenReadStream();
                command.ImageExtension = Path.GetExtension(model.Image.FileName);
            }

            try
            {
                await _mediator.Send(command);
                TempData["SuccessMessage"] = "تم حفظ التعديلات بنجاح.";
                return RedirectToAction(nameof(Details), new { id = model.Id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, FriendlyMessage(ex));
                model.Categories = await GetCategoriesAsync(model.CategoryId);
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var product = await _mediator.Send(new RetrieveProductCommand(id));
                ViewData["Title"] = "حذف المنتج";
                return View(product);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "المنتج المطلوب غير موجود.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            try
            {
                var result = await _mediator.Send(new DiscontinueProductCommand(id));
                if (result.Succeeded)
                    TempData["SuccessMessage"] = "تم إيقاف المنتج بنجاح.";
                else
                    TempData["ErrorMessage"] = result.Message;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = FriendlyMessage(ex);
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<Dictionary<Guid, string>> GetCategoriesAsync(Guid? includeId = null)
        {
            var response = await _mediator.Send(new GetAllProductCategoriesQuery());
            var categories = response?.Data ?? Enumerable.Empty<ProductCategoryDto>();
            return categories
                .Where(c => c.IsActive || c.Id == includeId)
                .ToDictionary(c => c.Id, c => c.Name);
        }

        private static string FriendlyMessage(Exception ex) => ex switch
        {
            BusinessException => "حدث خطأ أثناء تنفيذ العملية. يرجى التحقق من البيانات والمحاولة مرة أخرى.",
            KeyNotFoundException or InvalidOperationException => "البيانات المطلوبة غير موجودة.",
            ArgumentException => "بعض الحقول غير صحيحة. يرجى مراجعة البيانات.",
            _ => "حدث خطأ غير متوقع. يرجى المحاولة مرة أخرى."
        };
    }
}