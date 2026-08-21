using Application.Common.Interfaces;
using Application.QueryRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Models.StockMovements;

namespace MVC.Controllers
{
    [Authorize]
    public class StockMovementsController : Controller
    {
        private readonly IStockMovementQueryRepository _movementQuery;
        private readonly IProductQueryRepository _productQuery;
        private readonly IWarehouseQueryRepository _warehouseQuery;
        private readonly ICurrentUserContext _currentUser;

        public StockMovementsController(
            IStockMovementQueryRepository movementQuery,
            IProductQueryRepository productQuery,
            IWarehouseQueryRepository warehouseQuery,
            ICurrentUserContext currentUser)
        {
            _movementQuery = movementQuery;
            _productQuery = productQuery;
            _warehouseQuery = warehouseQuery;
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var brandId = _currentUser.ActiveBrandId;

            var movements = await _movementQuery.GetAllByBrandIdAsync(brandId);
            var products = (await _productQuery.GetAllByBrandIdAsync(brandId))
                .ToDictionary(p => p.Id, p => p);
            var warehouses = (await _warehouseQuery.GetAllByBrandIdAsync(brandId))
                .ToDictionary(w => w.Id, w => w);

            var rows = movements
                .OrderByDescending(m => m.CreatedAt)
                .Select(m => new StockMovementRowModel
                {
                    MovementDate = m.CreatedAt,
                    Type = m.MovementType,
                    TypeLabel = MovementLabel(m.MovementType, out var inbound),
                    IsInbound = inbound,
                    ReferenceLabel = ReferenceLabel(m.ReferenceType),
                    ProductName = products.TryGetValue(m.ProductId, out var p) ? p.Name : "منتج محذوف",
                    WarehouseName = warehouses.TryGetValue(m.WarehouseId, out var w) ? w.Name : "مخزن محذوف",
                    Quantity = m.Quantity
                })
                .ToList();

            var model = new StockMovementListModel
            {
                Movements = rows,
                TotalMovements = rows.Count,
                TotalIn = rows.Where(r => r.IsInbound).Sum(r => r.Quantity),
                TotalOut = rows.Where(r => !r.IsInbound).Sum(r => r.Quantity)
            };

            ViewData["Title"] = "حركة المخزون";
            return View(model);
        }

        private static string MovementLabel(string type, out bool inbound)
        {
            switch (type)
            {
                case "PurchaseIn": inbound = true; return "شراء وارد";
                case "TransferIn": inbound = true; return "تحويل وارد";
                case "AdjustmentIn": inbound = true; return "تسوية زيادة";
                case "OrderReturn": inbound = true; return "مرتجع بيع";
                case "SaleOut": inbound = false; return "بيع";
                case "TransferOut": inbound = false; return "تحويل صادر";
                case "AdjustmentOut": inbound = false; return "تسوية نقص";
                default: inbound = true; return type;
            }
        }

        private static string? ReferenceLabel(string? referenceType) => referenceType switch
        {
            "0" => "فاتورة شراء",
            "1" => "فاتورة بيع",
            "2" => "تحويل مخزون",
            "3" => "تسوية",
            "4" => "مرتجع",
            _ => null
        };
    }
}