using Application.QueryRepositories;
using Application.UseCases.SupplierCases;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Models.Suppliers;

namespace MVC.Controllers
{
    [Authorize]
    public class SuppliersController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ISupplierQueryRepository _supplierQuery;
        private readonly Application.Common.Interfaces.ICurrentUserContext _currentUser;

        public SuppliersController(IMediator mediator, ISupplierQueryRepository supplierQuery, Application.Common.Interfaces.ICurrentUserContext currentUser)
        {
            _mediator = mediator;
            _supplierQuery = supplierQuery;
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var brandId = _currentUser.ActiveBrandId;
            var suppliers = (await _supplierQuery.GetAllByBrandIdAsync(brandId))
                .OrderBy(s => s.Name)
                .Select(s => new SupplierRowModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Phone = s.Phone,
                    Email = s.Email,
                    Address = s.Address
                })
                .ToList();

            ViewData["Title"] = "الموردين";
            return View(suppliers);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["Title"] = "إضافة مورد جديد";
            return View(new SupplierCreateModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SupplierCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Title"] = "إضافة مورد جديد";
                return View(model);
            }

            var command = new RegisterSupplierCommand
            {
                Name = model.Name,
                Phone = model.Phone,
                Email = model.Email,
                Address = model.Address
            };

            try
            {
                var result = await _mediator.Send(command);
                TempData["SuccessMessage"] = "تم إضافة المورد بنجاح.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "حدث خطأ أثناء إنشاء المورد.");
                ViewData["Title"] = "إضافة مورد جديد";
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var dto = await _supplierQuery.GetByIdAsync(id);
            if (dto == null)
            {
                TempData["ErrorMessage"] = "المورد غير موجود.";
                return RedirectToAction(nameof(Index));
            }

            var model = new SupplierEditModel
            {
                Id = dto.Id,
                Name = dto.Name,
                Phone = dto.Phone,
                Email = dto.Email,
                Address = dto.Address
            };

            ViewData["Title"] = "تعديل مورد";
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SupplierEditModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Title"] = "تعديل مورد";
                return View(model);
            }

            var command = new Application.UseCases.SupplierCases.UpdateSupplierProfileCommand
            {
                Id = model.Id,
                Name = model.Name,
                Phone = model.Phone,
                Email = model.Email,
                Address = model.Address
            };

            try
            {
                await _mediator.Send(command);
                TempData["SuccessMessage"] = "تم حفظ التعديلات.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "حدث خطأ أثناء حفظ التعديلات.");
                ViewData["Title"] = "تعديل مورد";
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var dto = await _supplierQuery.GetByIdAsync(id);
            if (dto == null)
            {
                TempData["ErrorMessage"] = "المورد غير موجود.";
                return RedirectToAction(nameof(Index));
            }

            var model = new SupplierDetailsModel
            {
                Id = dto.Id,
                Name = dto.Name,
                Phone = dto.Phone,
                Email = dto.Email,
                Address = dto.Address
            };

            ViewData["Title"] = "تفاصيل المورد";
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var dto = await _supplierQuery.GetByIdAsync(id);
            if (dto == null)
            {
                TempData["ErrorMessage"] = "المورد غير موجود.";
                return RedirectToAction(nameof(Index));
            }

            var model = new SupplierDetailsModel
            {
                Id = dto.Id,
                Name = dto.Name,
                Phone = dto.Phone,
                Email = dto.Email,
                Address = dto.Address
            };

            ViewData["Title"] = "حذف مورد";
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            try
            {
                await _mediator.Send(new Application.UseCases.SupplierCases.RemoveSupplierCommand { Id = id });
                TempData["SuccessMessage"] = "تم حذف المورد.";
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء حذف المورد.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
