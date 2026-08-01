using Application.Common.Interfaces;
using Application.Dtos.Products;
using Application.QueryRepositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardQueryRepository _dashboardQuery;
        private readonly IProductQueryRepository _productQuery;
        private readonly ICurrentUserContext _currentUser;
        public DashboardController(
            IDashboardQueryRepository dashboardQuery,
            IProductQueryRepository productQuery,
            ICurrentUserContext currentUser)
        {
            _dashboardQuery = dashboardQuery;
            _productQuery = productQuery;
            _currentUser = currentUser;
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats(CancellationToken cancellationToken)
        {
            var brandId = _currentUser.ActiveBrandId;
            if (brandId == Guid.Empty)
                return Unauthorized();

            var stats = await _dashboardQuery.GetDashboardStatsAsync(brandId);
            return Ok(stats);
        }

        [HttpGet("products-stock")]
        public async Task<IActionResult> GetProductsWithStock(CancellationToken cancellationToken)
        {
            var brandId = _currentUser.ActiveBrandId;
            if (brandId == Guid.Empty)
                return Unauthorized();

            var products = await _productQuery.GetProductsWithQuantities(brandId);
            return Ok(products);
        }

        [HttpGet("products-stock-by-warehouse")]
        public async Task<IActionResult> GetProductsStockByWarehouse(CancellationToken cancellationToken)
        {
            var brandId = _currentUser.ActiveBrandId;
            if (brandId == Guid.Empty)
                return Unauthorized();

            var products = await _productQuery.GetAllProductStockByWarehouseAsync(brandId);
            return Ok(products);
        }

        [HttpGet("low-stock")]
        public async Task<IActionResult> GetLowStock([FromQuery] int threshold = 10, CancellationToken cancellationToken = default)
        {
            var brandId = _currentUser.ActiveBrandId;
            if (brandId == Guid.Empty)
                return Unauthorized();

            var products = await _productQuery.GetLowStockProductsAsync(brandId, threshold);
            return Ok(products);
        }
    }
}