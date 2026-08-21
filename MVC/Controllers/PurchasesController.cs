using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.QueryRepositories;
using Application.UseCases.Purchase;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Models.Purchases;
using MVC.Models.Warehouses;

namespace MVC.Controllers
{
    [Authorize]
    public class PurchasesController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IPurchaseQueryRepository _purchaseQuery;
        private readonly ISupplierQueryRepository _supplierQuery;
        private readonly IProductQueryRepository _productQuery;
        private readonly IWarehouseQueryRepository _warehouseQuery;
        private readonly ICurrentUserContext _currentUser;

        public PurchasesController(
            IMediator mediator,
            IPurchaseQueryRepository purchaseQuery,
            ISupplierQueryRepository supplierQuery,
            IProductQueryRepository productQuery,
            IWarehouseQueryRepository warehouseQuery,
            ICurrentUserContext currentUser)
        {
            _mediator = mediator;
            _purchaseQuery = purchaseQuery;
            _supplierQuery = supplierQuery;
            _productQuery = productQuery;
            _warehouseQuery = warehouseQuery;
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? status = null)
        {
            var brandId = _currentUser.ActiveBrandId;

            var purchases = await _purchaseQuery.GetAllWithItemsByBrandIdAsync(brandId);
            var suppliers = (await _supplierQuery.GetAllByBrandIdAsync(brandId))
                .ToDictionary(s => s.Id, s => s.Name);

            var rows = purchases
                .Where(p => string.IsNullOrEmpty(status) || string.Equals(p.Status, status, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(p => p.PurchaseDate)
                .Select(p => new PurchaseRowModel
                {
                    Id = p.Id,
                    PurchaseDate = p.PurchaseDate,
                    Status = p.Status,
                    StatusLabel = PurchaseStatusLabel(p.Status),
                    TotalAmount = p.TotalAmount,
                    SupplierName = suppliers.TryGetValue(p.SupplierId, out var sn) ? sn : "—",
                    ItemCount = p.Items.Count,
                    TotalUnits = p.Items.Sum(i => i.Quantity),
                    ReceivedUnits = p.Items.Sum(i => i.ReceivedQuantity)
                })
                .ToList();

            var model = new PurchaseListModel
            {
                Purchases = rows,
                TotalOrders = rows.Count,
                TotalOrdered = rows.Sum(p => p.TotalAmount),
                StatusFilter = status
            };

            ViewData["Title"] = "أوامر الشراء";
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var brandId = _currentUser.ActiveBrandId;
            var model = new PurchaseCreateModel
            {
                Suppliers = (await _supplierQuery.GetAllByBrandIdAsync(brandId)).OrderBy(s => s.Name).ToList(),
                Products = (await _productQuery.GetAllByBrandIdAsync(brandId)).Where(p => p.IsActive).OrderBy(p => p.Name).ToList()
            };
            ViewData["Title"] = "أمر شراء جديد";
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PurchaseCreateModel model)
        {
            if (model.SupplierId == Guid.Empty)
                ModelState.AddModelError("SupplierId", "اختر المورد.");

            if (model.Lines == null || !model.Lines.Any(l => l.Quantity > 0))
                ModelState.AddModelError(string.Empty, "أضف صنفاً واحداً على الأقل بكمية أكبر من صفر.");

            if (!ModelState.IsValid)
            {
                await PopulateCreateListsAsync(model);
                ViewData["Title"] = "أمر شراء جديد";
                return View(model);
            }

            var command = new CreatePurchaseOrderCommand
            {
                SupplierId = model.SupplierId,
                PurchaseDate = model.PurchaseDate,
                Lines = model.Lines
                    .Where(l => l.Quantity > 0)
                    .Select(l => new PurchaseOrderLineDto
                    {
                        ProductId = l.ProductId,
                        Quantity = l.Quantity,
                        UnitCost = l.UnitCost
                    })
                    .ToList()
            };

            try
            {
                var created = await _mediator.Send(command);
                TempData["SuccessMessage"] = $"تم إنشاء أمر الشراء بنجاح. استلم الدفعات لتبدأ الكميات في المخازن.";
                return RedirectToAction(nameof(Details), new { id = created.Id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, FriendlyMessage(ex));
                await PopulateCreateListsAsync(model);
                ViewData["Title"] = "أمر شراء جديد";
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var purchase = await _purchaseQuery.GetByIdWithItemsAsync(id);
            if (purchase == null)
            {
                TempData["ErrorMessage"] = "أمر الشراء المطلوب غير موجود.";
                return RedirectToAction(nameof(Index));
            }

            var brandId = _currentUser.ActiveBrandId;
            var products = (await _productQuery.GetAllByBrandIdAsync(brandId))
                .ToDictionary(p => p.Id, p => p);
            var suppliers = (await _supplierQuery.GetAllByBrandIdAsync(brandId))
                .ToDictionary(s => s.Id, s => s.Name);
            var warehouses = (await _warehouseQuery.GetAllByBrandIdAsync(brandId))
                .OrderBy(w => w.Name)
                .Select(w => new ReceiveWarehouseModel
                {
                    Id = w.Id,
                    Name = w.Name,
                    TypeLabel = WarehouseTypeLabels.Label(w.Type)
                })
                .ToList();

            var items = purchase.Items.Select(i =>
            {
                products.TryGetValue(i.ProductId, out var p);
                return new PurchaseItemRowModel
                {
                    PurchaseItemId = i.Id,
                    ProductId = i.ProductId,
                    ProductName = p?.Name ?? "منتج محذوف",
                    Barcode = p?.Barcode,
                    Quantity = i.Quantity,
                    ReceivedQuantity = i.ReceivedQuantity,
                    RemainingToReceive = i.Quantity - i.ReceivedQuantity,
                    UnitCost = i.UnitCost
                };
            }).ToList();

            var receiveLines = items
                .Where(i => i.RemainingToReceive > 0 && purchase.Status == "Ordered")
                .Select(i => new ReceiveLineModel
                {
                    PurchaseItemId = i.PurchaseItemId,
                    Quantity = i.RemainingToReceive,
                    UnitCost = i.UnitCost,
                    Allocations = warehouses.Select(w => new ReceiveAllocationModel
                    {
                        WarehouseId = w.Id,
                        WarehouseName = w.Name,
                        Quantity = 0
                    }).ToList()
                })
                .ToList();

            var model = new PurchaseDetailsModel
            {
                Id = purchase.Id,
                PurchaseDate = purchase.PurchaseDate,
                Status = purchase.Status,
                StatusLabel = PurchaseStatusLabel(purchase.Status),
                TotalAmount = purchase.TotalAmount,
                SupplierName = suppliers.TryGetValue(purchase.SupplierId, out var sn) ? sn : "—",
                CanReceive = purchase.Status == "Ordered",
                CanCancel = purchase.Status == "Ordered",
                Items = items,
                ReceiveWarehouses = warehouses,
                ReceiveForm = new ReceivePurchaseFormModel
                {
                    PurchaseId = purchase.Id,
                    Lines = receiveLines
                }
            };

            ViewData["Title"] = $"أمر شراء #{model.Id.ToString("N")[..8].ToUpper()}";
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Receive(ReceivePurchaseFormModel form)
        {
            var command = new ReceivePurchaseBatchCommand
            {
                PurchaseId = form.PurchaseId,
                Lines = form.Lines
                    .Where(l => l.Quantity > 0)
                    .Select(l => new ReceivePurchaseLineDto
                    {
                        PurchaseItemId = l.PurchaseItemId,
                        Quantity = l.Quantity,
                        UnitCost = l.UnitCost,
                        Allocations = l.Allocations
                            .Where(a => a.Quantity > 0)
                            .Select(a => new WarehouseAllocationDto
                            {
                                WarehouseId = a.WarehouseId,
                                Quantity = a.Quantity
                            })
                            .ToList()
                    })
                    .ToList()
            };

            try
            {
                var updated = await _mediator.Send(command);
                TempData["SuccessMessage"] = "تم استلام الدفعة وإضافتها إلى المخازن بنجاح.";
                return RedirectToAction(nameof(Details), new { id = updated.Id });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = FriendlyMessage(ex);
                return RedirectToAction(nameof(Details), new { id = form.PurchaseId });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(Guid id)
        {
            try
            {
                var updated = await _mediator.Send(new CancelPurchaseOrderCommand { Id = id });
                TempData["SuccessMessage"] = "تم إلغاء أمر الشراء.";
                return RedirectToAction(nameof(Details), new { id = updated.Id });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = FriendlyMessage(ex);
                return RedirectToAction(nameof(Details), new { id });
            }
        }

        private async Task PopulateCreateListsAsync(PurchaseCreateModel model)
        {
            var brandId = _currentUser.ActiveBrandId;
            model.Suppliers = (await _supplierQuery.GetAllByBrandIdAsync(brandId)).OrderBy(s => s.Name).ToList();
            model.Products = (await _productQuery.GetAllByBrandIdAsync(brandId)).Where(p => p.IsActive).OrderBy(p => p.Name).ToList();
        }

        internal static string PurchaseStatusLabel(string status) => status switch
        {
            "Ordered" => "قيد التوريد",
            "Completed" => "مكتمل",
            "Cancelled" => "ملغي",
            _ => status
        };

        private static string FriendlyMessage(Exception ex) => ex switch
        {
            BadRequestException => ex.Message,
            NotFoundException => "أمر الشراء المطلوب غير موجود.",
            _ => "حدث خطأ غير متوقع. يرجى المحاولة مرة أخرى."
        };
    }
}