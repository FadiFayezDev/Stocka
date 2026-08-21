using Application.Common.Exceptions;
using Application.Dtos.Products;
using Application.Features.Queries.ProductCategory.GetAll;
using Application.Features.Queries.ProductCategory.GetById;
using Application.UseCases.Category;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Models.Products;

namespace MVC.Controllers
{
    [Authorize(Roles = "BrandOwner")]
    public class ProductCategoriesController : Controller
    {
        private readonly IMediator _mediator;

        public ProductCategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categories = await _mediator.Send(new ListCategoriesCommand());
            ViewData["Title"] = "فئات المنتجات";
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["Title"] = "إضافة فئة جديدة";
            return View(new CategoryFormModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryFormModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                await _mediator.Send(new RegisterProductCategoryCommand { Name = model.Name });
                TempData["SuccessMessage"] = "تم إضافة الفئة بنجاح.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, FriendlyMessage(ex));
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                var response = await _mediator.Send(new GetProductCategoryByIdQuery(id));
                if (response is not { Succeeded: true } || response.Data is null)
                {
                    TempData["ErrorMessage"] = "الفئة المطلوبة غير موجودة.";
                    return RedirectToAction(nameof(Index));
                }

                var model = new CategoryFormModel
                {
                    Id = response.Data.Id,
                    Name = response.Data.Name,
                    IsActive = response.Data.IsActive
                };

                ViewData["Title"] = "تعديل الفئة";
                return View(model);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "الفئة المطلوبة غير موجودة.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryFormModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var result = await _mediator.Send(new UpdateProductCategoryCommand
                {
                    Id = model.Id,
                    Name = model.Name,
                    IsActive = model.IsActive
                });

                if (!result.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, result.Message);
                    return View(model);
                }

                TempData["SuccessMessage"] = "تم حفظ التعديلات بنجاح.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, FriendlyMessage(ex));
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var response = await _mediator.Send(new GetProductCategoryByIdQuery(id));
                if (response is not { Succeeded: true } || response.Data is null)
                {
                    TempData["ErrorMessage"] = "الفئة المطلوبة غير موجودة.";
                    return RedirectToAction(nameof(Index));
                }

                ViewData["Title"] = "حذف الفئة";
                return View(response.Data);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "الفئة المطلوبة غير موجودة.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            try
            {
                var result = await _mediator.Send(new DeleteProductCategoryCommand(id));
                if (result.Succeeded)
                    TempData["SuccessMessage"] = "تم حذف الفئة بنجاح.";
                else
                    TempData["ErrorMessage"] = result.Message;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = FriendlyMessage(ex);
            }

            return RedirectToAction(nameof(Index));
        }

        private static string FriendlyMessage(Exception ex) => ex switch
        {
            BusinessException => "حدث خطأ أثناء تنفيذ العملية. تأكد من عدم ارتباط الفئة بأي منتجات ثم حاول مرة أخرى.",
            KeyNotFoundException or InvalidOperationException => "البيانات المطلوبة غير موجودة.",
            ArgumentException => "اسم الفئة غير صحيح. يرجى مراجعة البيانات.",
            _ => "حدث خطأ غير متوقع. يرجى المحاولة مرة أخرى."
        };
    }
}