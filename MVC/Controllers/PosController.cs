using Application.Common.Interfaces;
using Application.Features.Queries.ProductCategory.GetAll;
using Application.QueryRepositories;
using Application.UseCases.ProductCases;
using Application.UseCases.SaleCases;
using Domain.Contracts;
using Domain.Entities.Core;
using Domain.Primitives;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Models.Pos;

namespace MVC.Controllers
{
    [Authorize]
    public class PosController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IProductQueryRepository _productQuery;
        private readonly ICustomerQueryRepository _customerQuery;
        private readonly IEmployeeQueryRepository _employeeQuery;
        private readonly IOrderQueryRepository _orderQuery;
        private readonly IEmployeeCommandRepository _employeeCommand;
        private readonly IBranchQueryRepository _branchQuery;
        private readonly IWarehouseQueryRepository _warehouseQuery;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserContext _currentUser;

        public PosController(
            IMediator mediator,
            IProductQueryRepository productQuery,
            ICustomerQueryRepository customerQuery,
            IEmployeeQueryRepository employeeQuery,
            IOrderQueryRepository orderQuery,
            IEmployeeCommandRepository employeeCommand,
            IBranchQueryRepository branchQuery,
            IWarehouseQueryRepository warehouseQuery,
            IUnitOfWork unitOfWork,
            ICurrentUserContext currentUser)
        {
            _mediator = mediator;
            _productQuery = productQuery;
            _customerQuery = customerQuery;
            _employeeQuery = employeeQuery;
            _orderQuery = orderQuery;
            _employeeCommand = employeeCommand;
            _branchQuery = branchQuery;
            _warehouseQuery = warehouseQuery;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> Index(Guid? warehouse = null)
        {
            var brandId = _currentUser.ActiveBrandId;

            var sellingWarehouses = (await _warehouseQuery.GetAllByBrandIdAsync(brandId))
                .Where(w => string.Equals(w.Type, "Shop", StringComparison.OrdinalIgnoreCase))
                .OrderBy(w => w.Name)
                .ToList();

            Guid? selectedWarehouseId = null;

            if (warehouse.HasValue && warehouse.Value != Guid.Empty)
            {
                var chosen = sellingWarehouses.FirstOrDefault(w => w.Id == warehouse.Value);
                if (chosen != null)
                    selectedWarehouseId = chosen.Id;
            }

            if (selectedWarehouseId == null && _currentUser.ActiveBranchId.HasValue)
            {
                var branchWarehouses = await _warehouseQuery.GetByBranchIdAsync(_currentUser.ActiveBranchId.Value);
                selectedWarehouseId = branchWarehouses
                    .Where(w => string.Equals(w.Type, "Shop", StringComparison.OrdinalIgnoreCase))
                    .Select(w => (Guid?)w.Id)
                    .FirstOrDefault();
            }

            if (selectedWarehouseId == null)
                selectedWarehouseId = sellingWarehouses.Select(w => (Guid?)w.Id).FirstOrDefault();

            var model = new PosIndexModel
            {
                Products = (await _productQuery.GetProductsWithQuantities(brandId, selectedWarehouseId)).ToList(),
                Customers = (await _customerQuery.GetAllByBrandIdAsync(brandId)).ToList(),
                Categories = (await _mediator.Send(new GetAllProductCategoriesQuery()))
                    .Data?.Where(c => c.IsActive).ToList() ?? new(),
                RecentOrders = (await _orderQuery.GetAllWithItemsByBrandIdAsync(brandId))
                    .Where(o => o.OrderDate.ToLocalTime().Date == DateTime.Today)
                    .OrderByDescending(o => o.OrderDate)
                    .Take(10)
                    .ToList(),
                SellingWarehouses = sellingWarehouses,
                SelectedWarehouseId = selectedWarehouseId
            };

            model.EmployeeName = User.Identity?.Name;

            ViewData["Title"] = "نقطة البيع";

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout([FromBody] PosCheckoutModel model)
        {
            if (model.Items == null || !model.Items.Any())
                return Json(new { success = false, message = "السلة فارغة. أضف منتجات قبل إتمام البيع." });

            model.Items = model.Items.Where(i => i.ProductId != Guid.Empty && i.Quantity > 0).ToList();
            if (!model.Items.Any())
                return Json(new { success = false, message = "البيانات غير صحيحة. تأكد من الكميات." });

            foreach (var item in model.Items)
            {
                if (item.Quantity <= 0)
                    return Json(new { success = false, message = "الكميات يجب أن تكون أكبر من صفر." });
                if (item.UnitPrice <= 0)
                    return Json(new { success = false, message = "سعر البيع يجب أن يكون أكبر من صفر." });
            }

            var employeeId = await ResolveEmployeeIdAsync();

            if (employeeId == Guid.Empty)
                return Json(new { success = false, message = "لم يتم العثور على موظف مرتبط بحسابك. تواصل مع مدير النظام." });

            // تطبيق الخصم على مستوى سطور الفاتورة (تناسبياً) حتى لا نخالف
            // قاعدة: سعر البيع لا يقل عن سعر التكلفة.
            ApplyDiscount(model);

            var command = new RecordSaleCommand
            {
                EmployeeId = employeeId,
                CustomerId = model.CustomerId,
                Notes = string.IsNullOrWhiteSpace(model.Notes) ? null : model.Notes.Trim(),
                WarehouseId = model.WarehouseId,
                Items = model.Items.Select(i => new SaleItemDto
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
            };

            try
            {
                var result = await _mediator.Send(command);

                if (!result.Succeeded)
                    return Json(new { success = false, message = result.Message });

                var redirectUrl = Url.Action(nameof(Receipt), "Pos", new
                {
                    id = result.Data!.OrderId,
                    received = model.ReceivedAmount,
                    discount = model.Discount
                });

                return Json(new
                {
                    success = true,
                    message = "تم إتمام البيع بنجاح.",
                    orderId = result.Data.OrderId,
                    total = result.Data.TotalAmount,
                    redirectUrl
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = FriendlyMessage(ex) });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Receipt(Guid id, decimal received = 0, decimal discount = 0)
        {
            var order = await _orderQuery.GetByIdWithItemsAsync(id);
            if (order == null)
            {
                TempData["ErrorMessage"] = "الفاتورة غير موجودة.";
                return RedirectToAction(nameof(Index));
            }

            var products = await _mediator.Send(new ListProductsCommand());

            var model = new ReceiptModel
            {
                Order = order,
                ProductNames = products.ToDictionary(p => p.Id, p => p.Name),
                ReceivedAmount = received,
                Discount = discount,
                EmployeeName = User.Identity?.Name
            };

            ViewData["Title"] = "فاتورة بيع";
            ViewData["Print"] = true;

            return View(model);
        }

        private async Task<Guid> ResolveEmployeeIdAsync()
        {
            var employee = await _employeeQuery.GetByUserIdAsync(_currentUser.UserId);
            if (employee != null)
                return employee.Id;

            // لا يوجد موظف مرتبط بحساب المستخدم (مثل صاحب العلامة عند
            // التسجيل الأول). ننشئ سجلاً افتراضياً تلقائياً ومرة واحدة.
            var brandId = _currentUser.ActiveBrandId;
            var branchId = _currentUser.ActiveBranchId;

            if (branchId == null || branchId == Guid.Empty)
            {
                var firstBranch = (await _branchQuery.GetAllByBrandIdAsync(brandId)).FirstOrDefault();
                branchId = firstBranch?.Id;
            }

            if (branchId == null || branchId == Guid.Empty)
                return Guid.Empty;

            var newEmployee = new Employee(
                _currentUser.UserId,
                new BrandId(brandId),
                "صاحب العلامة",
                null,
                new BranchId(branchId.Value));

            await _employeeCommand.CreateAsync(newEmployee);
            await _unitOfWork.SaveChangesAsync();

            return newEmployee.Id.Value;
        }

        private static void ApplyDiscount(PosCheckoutModel model)
        {
            if (model.Discount <= 0)
            {
                model.Discount = 0;
                return;
            }

            var subtotal = model.Items.Sum(i => i.Quantity * i.UnitPrice);
            if (subtotal <= 0)
            {
                model.Discount = 0;
                return;
            }

            // لا نسمح بخصم يقل بالمنتج عن سعر التكلفة — لا نملك التكلفة هنا
            // لذلك نحد الخصم بـ 90% كحد أقصى أماناً (الفحص النهائي بالـ domain).
            var maxDiscount = subtotal * 0.90m;
            if (model.Discount > maxDiscount)
                model.Discount = maxDiscount;

            var ratio = (subtotal - model.Discount) / subtotal;

            for (var i = 0; i < model.Items.Count; i++)
            {
                var item = model.Items[i];
                var discountedTotal = Math.Round(item.Quantity * item.UnitPrice * ratio, 2);

                // نضع الباقي الناتج عن التقريب على آخر سطر ليظل الإجمالي مطابقاً.
                if (i == model.Items.Count - 1)
                {
                    var applied = model.Items.Take(i).Sum(x => Math.Round(x.Quantity * x.UnitPrice * ratio, 2));
                    discountedTotal = Math.Round(subtotal - model.Discount - applied, 2);
                }

                item.UnitPrice = item.Quantity > 0
                    ? Math.Round(discountedTotal / item.Quantity, 2)
                    : item.UnitPrice;
            }
        }

        private static string FriendlyMessage(Exception ex) => ex switch
        {
            Application.Common.Exceptions.BusinessException => "حدث خطأ أثناء تنفيذ العملية. يرجى التحقق من البيانات والمحاولة مرة أخرى.",
            KeyNotFoundException or InvalidOperationException => "البيانات المطلوبة غير موجودة.",
            ArgumentException => "بعض الحقول غير صحيحة. يرجى مراجعة البيانات.",
            _ => "حدث خطأ غير متوقع. يرجى المحاولة مرة أخرى."
        };
    }
}