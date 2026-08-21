using Application.Common.Interfaces;
using Application.QueryRepositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Models.Orders;

namespace MVC.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IOrderQueryRepository _orderQuery;
        private readonly IProductQueryRepository _productQuery;
        private readonly IBranchQueryRepository _branchQuery;
        private readonly ICurrentUserContext _currentUser;

        public OrdersController(
            IOrderQueryRepository orderQuery,
            IProductQueryRepository productQuery,
            IBranchQueryRepository branchQuery,
            ICurrentUserContext currentUser)
        {
            _orderQuery = orderQuery;
            _productQuery = productQuery;
            _branchQuery = branchQuery;
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var brandId = _currentUser.ActiveBrandId;

            var orders = await _orderQuery.GetAllWithItemsByBrandIdAsync(brandId);
            var branches = (await _branchQuery.GetAllByBrandIdAsync(brandId))
                .ToDictionary(b => b.Id, b => b.Name);

            var rows = orders
                .OrderByDescending(o => o.OrderDate)
                .Select(o => new OrderRowModel
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    Status = o.Status,
                    TotalAmount = o.TotalAmount,
                    BranchName = o.BranchId.HasValue && branches.TryGetValue(o.BranchId.Value, out var bn) ? bn : null,
                    ItemCount = o.Items.Count,
                    TotalUnits = o.Items.Sum(i => i.Quantity)
                })
                .ToList();

            var model = new OrderListModel
            {
                Orders = rows,
                TotalOrders = rows.Count,
                TotalSales = rows.Sum(o => o.TotalAmount)
            };

            ViewData["Title"] = "المبيعات";
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var order = await _orderQuery.GetByIdWithItemsAsync(id);
            if (order == null)
            {
                TempData["ErrorMessage"] = "الفاتورة المطلوبة غير موجودة.";
                return RedirectToAction(nameof(Index));
            }

            var brandId = _currentUser.ActiveBrandId;
            var products = (await _productQuery.GetAllByBrandIdAsync(brandId))
                .ToDictionary(p => p.Id, p => p);
            var branches = (await _branchQuery.GetAllByBrandIdAsync(brandId))
                .ToDictionary(b => b.Id, b => b.Name);

            var model = new OrderDetailsModel
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                BranchName = order.BranchId.HasValue && branches.TryGetValue(order.BranchId.Value, out var bn) ? bn : null,
                TotalUnits = order.Items.Sum(i => i.Quantity),
                Items = order.Items
                    .Select(i =>
                    {
                        products.TryGetValue(i.ProductId, out var p);
                        return new OrderItemRowModel
                        {
                            ProductId = i.ProductId,
                            ProductName = p?.Name ?? "منتج محذوف",
                            Barcode = p?.Barcode,
                            Quantity = i.Quantity,
                            UnitPrice = i.UnitPrice,
                            CostPrice = i.CostPrice
                        };
                    })
                    .ToList()
            };

            ViewData["Title"] = $"فاتورة بيع #{model.Id.ToString("N")[..8].ToUpper()}";
            return View(model);
        }
    }
}