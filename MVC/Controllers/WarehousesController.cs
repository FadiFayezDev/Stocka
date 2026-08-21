using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Products;
using Application.QueryRepositories;
using Application.UseCases.ProductCases;
using Application.UseCases.WarehouseCases;
using Domain.Repositories.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Models.Warehouses;

namespace MVC.Controllers
{
    [Authorize]
    public class WarehousesController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IProductQueryRepository _productQuery;
        private readonly IBranchQueryRepository _branchQuery;
        private readonly IWarehouseQueryRepository _warehouseQuery;
        private readonly IWarehouseCommandRepository _warehouseCommand;
        private readonly ICurrentUserContext _currentUser;

        public WarehousesController(
            IMediator mediator,
            IProductQueryRepository productQuery,
            IBranchQueryRepository branchQuery,
            IWarehouseQueryRepository warehouseQuery,
            IWarehouseCommandRepository warehouseCommand,
            ICurrentUserContext currentUser)
        {
            _mediator = mediator;
            _productQuery = productQuery;
            _branchQuery = branchQuery;
            _warehouseQuery = warehouseQuery;
            _warehouseCommand = warehouseCommand;
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var warehouses = await _mediator.Send(new ListWarehousesCommand());
            var stock = await _productQuery.GetAllProductStockByWarehouseAsync(_currentUser.ActiveBrandId);
            var branchNames = await _warehouseQuery.GetBranchNamesByWarehouseIdsAsync(_currentUser.ActiveBrandId);

            var summaries = stock
                .GroupBy(s => s.WarehouseId)
                .ToDictionary(
                    g => g.Key,
                    g => new WarehouseSummaryModel
                    {
                        ProductCount = g.Select(x => x.ProductId).Distinct().Count(),
                        TotalUnits = g.Sum(x => x.Quantity)
                    });

            ViewData["Title"] = "المخازن";
            return View(new WarehouseIndexModel { Warehouses = warehouses, Summaries = summaries, BranchNames = branchNames });
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new WarehouseFormModel();
            await PopulateBranchesAsync(model);
            ViewData["Title"] = "إضافة مخزن";
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WarehouseFormModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateBranchesAsync(model);
                ViewData["Title"] = "إضافة مخزن";
                return View(model);
            }

            var command = new RegisterWarehouseCommand
            {
                Name = model.Name,
                Type = model.Type,
                Location = model.Location,
                Description = model.Description
            };

            try
            {
                var created = await _mediator.Send(command);

                await _mediator.Send(new AssignWarehouseBranchesCommand
                {
                    WarehouseId = created.Id,
                    BranchIds = model.SelectedBranchIds
                });

                TempData["SuccessMessage"] = "تم إنشاء المخزن بنجاح.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, FriendlyMessage(ex));
                await PopulateBranchesAsync(model);
                ViewData["Title"] = "إضافة مخزن";
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                var warehouse = await _mediator.Send(new RetrieveWarehouseCommand(id));

                var model = new WarehouseFormModel
                {
                    Id = warehouse.Id,
                    Name = warehouse.Name,
                    Type = WarehouseTypeLabels.Parse(warehouse.Type),
                    Location = warehouse.Location,
                    Description = warehouse.Description,
                    IsActive = warehouse.IsActive
                };

                await PopulateBranchesAsync(model);

                ViewData["Title"] = "تعديل المخزن";
                return View(model);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "المخزن المطلوب غير موجود.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(WarehouseFormModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateBranchesAsync(model);
                ViewData["Title"] = "تعديل المخزن";
                return View(model);
            }

            var command = new UpdateWarehouseInformationCommand
            {
                Id = model.Id,
                Name = model.Name,
                Type = model.Type,
                Location = model.Location,
                Description = model.Description,
                IsActive = model.IsActive
            };

            try
            {
                await _mediator.Send(command);

                await _mediator.Send(new AssignWarehouseBranchesCommand
                {
                    WarehouseId = model.Id,
                    BranchIds = model.SelectedBranchIds
                });

                TempData["SuccessMessage"] = "تم حفظ تعديلات المخزن بنجاح.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, FriendlyMessage(ex));
                await PopulateBranchesAsync(model);
                ViewData["Title"] = "تعديل المخزن";
                return View(model);
            }
        }

        private async Task PopulateBranchesAsync(WarehouseFormModel model)
        {
            var branches = await _branchQuery.GetAllByBrandIdAsync(_currentUser.ActiveBrandId);
            model.Branches = branches.OrderBy(b => b.Name).ToList();

            if (model.Id != Guid.Empty)
            {
                var linked = await _warehouseCommand.GetLinkedBranchIdsAsync(model.Id);
                model.SelectedBranchIds = linked.ToList();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Stock(Guid id)
        {
            WarehouseDto? warehouse = null;
            try
            {
                warehouse = await _mediator.Send(new RetrieveWarehouseCommand(id));
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "المخزن المطلوب غير موجود.";
                return RedirectToAction(nameof(Index));
            }

            var allStock = await _productQuery.GetAllProductStockByWarehouseAsync(_currentUser.ActiveBrandId);

            var items = allStock
                .Where(s => s.WarehouseId == id)
                .OrderBy(s => s.ProductName)
                .ToList();

            var model = new WarehouseStockModel
            {
                Warehouse = warehouse,
                Items = items,
                ProductCount = items.Select(i => i.ProductId).Distinct().Count(),
                TotalUnits = items.Sum(i => i.Quantity)
            };

            ViewData["Title"] = $"محتويات المخزن — {warehouse.Name}";
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Transfer(Guid? from = null)
        {
            var model = new TransferStockModel();
            await PopulateTransferListsAsync(model);
            if (from.HasValue && model.Warehouses.Any(w => w.Id == from.Value))
                model.FromWarehouseId = from.Value;
            ViewData["Title"] = "تحويل مخزون";
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Transfer(TransferStockModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateTransferListsAsync(model);
                ViewData["Title"] = "تحويل مخزون";
                return View(model);
            }

            var command = new TransferStockCommand
            {
                ProductId = model.ProductId,
                FromWarehouseId = model.FromWarehouseId,
                ToWarehouseId = model.ToWarehouseId,
                Quantity = model.Quantity,
                Notes = model.Notes
            };

            try
            {
                var result = await _mediator.Send(command);
                TempData["SuccessMessage"] = $"تم تحويل {result.MovedQuantity} وحدة من منتج «{result.ProductName}» بنجاح.";
                return RedirectToAction(nameof(Transfer));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, FriendlyMessage(ex));
                await PopulateTransferListsAsync(model);
                ViewData["Title"] = "تحويل مخزون";
                return View(model);
            }
        }

        private async Task PopulateTransferListsAsync(TransferStockModel model)
        {
            var warehouses = await _mediator.Send(new ListWarehousesCommand());
            var products = await _mediator.Send(new ListProductsCommand());

            model.Warehouses = warehouses
                .OrderBy(w => w.Type)
                .ThenBy(w => w.Name)
                .ToList();
            model.Products = products
                .Where(p => p.IsActive)
                .OrderBy(p => p.Name)
                .ToList();
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