using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.QueryRepositories;
using Application.UseCases.BranchCases;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Models.Branches;

namespace MVC.Controllers
{
    [Authorize]
    public class BranchesController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IBranchQueryRepository _branchQuery;
        private readonly IWarehouseQueryRepository _warehouseQuery;
        private readonly ICurrentUserContext _currentUser;

        public BranchesController(
            IMediator mediator,
            IBranchQueryRepository branchQuery,
            IWarehouseQueryRepository warehouseQuery,
            ICurrentUserContext currentUser)
        {
            _mediator = mediator;
            _branchQuery = branchQuery;
            _warehouseQuery = warehouseQuery;
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var branches = await _mediator.Send(new ListBranchesCommand());
            var warehouseNames = await _warehouseQuery.GetWarehouseNamesByBranchIdsAsync(_currentUser.ActiveBrandId);

            var model = new BranchIndexModel
            {
                Branches = branches.OrderBy(b => b.Name).ToList(),
                WarehouseNames = warehouseNames
            };

            ViewData["Title"] = "الفروع";
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new BranchFormModel();
            await PopulateWarehousesAsync(model);
            ViewData["Title"] = "إضافة فرع";
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BranchFormModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateWarehousesAsync(model);
                ViewData["Title"] = "إضافة فرع";
                return View(model);
            }

            try
            {
                var branch = await _mediator.Send(new RegisterBranchCommand { Name = model.Name });

                await _mediator.Send(new AssignBranchWarehousesCommand
                {
                    BranchId = branch.Id,
                    WarehouseIds = model.SelectedWarehouseIds
                });

                TempData["SuccessMessage"] = "تم إضافة الفرع بنجاح.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, FriendlyMessage(ex));
                await PopulateWarehousesAsync(model);
                ViewData["Title"] = "إضافة فرع";
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                var branch = await _mediator.Send(new RetrieveBranchCommand(id));

                var model = new BranchFormModel
                {
                    Id = branch.Id,
                    Name = branch.Name
                };

                await PopulateWarehousesAsync(model);

                ViewData["Title"] = "تعديل الفرع";
                return View(model);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "الفرع المطلوب غير موجود.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BranchFormModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateWarehousesAsync(model);
                ViewData["Title"] = "تعديل الفرع";
                return View(model);
            }

            try
            {
                await _mediator.Send(new UpdateBranchInformationCommand { Id = model.Id, Name = model.Name });

                await _mediator.Send(new AssignBranchWarehousesCommand
                {
                    BranchId = model.Id,
                    WarehouseIds = model.SelectedWarehouseIds
                });

                TempData["SuccessMessage"] = "تم حفظ تعديلات الفرع بنجاح.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, FriendlyMessage(ex));
                await PopulateWarehousesAsync(model);
                ViewData["Title"] = "تعديل الفرع";
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _mediator.Send(new RemoveBranchCommand { Id = id });
                TempData["SuccessMessage"] = "تم حذف الفرع بنجاح.";
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "تعذّر حذف الفرع.";
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateWarehousesAsync(BranchFormModel model)
        {
            var warehouses = await _warehouseQuery.GetAllByBrandIdAsync(_currentUser.ActiveBrandId);
            model.Warehouses = warehouses.OrderBy(w => w.Name).ToList();

            if (model.Id != Guid.Empty)
            {
                var linked = await _warehouseQuery.GetByBranchIdAsync(model.Id);
                model.SelectedWarehouseIds = linked.Select(w => w.Id).ToList();
            }
        }

        private static string FriendlyMessage(Exception ex) => ex switch
        {
            BadRequestException => ex.Message,
            NotFoundException => "أحد العناصر المطلوبة غير موجود.",
            BusinessException => "حدث خطأ أثناء تنفيذ العملية. يرجى التحقق من البيانات والمحاولة مرة أخرى.",
            KeyNotFoundException or InvalidOperationException => "البيانات المطلوبة غير موجودة.",
            ArgumentException => "بعض الحقول غير صحيحة. يرجى مراجعة البيانات.",
            _ => "حدث خطأ غير متوقع. يرجى المحاولة مرة أخرى."
        };
    }
}