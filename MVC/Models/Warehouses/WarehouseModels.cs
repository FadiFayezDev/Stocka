using Application.Dtos.Core;
using Application.Dtos.Products;
using System.ComponentModel.DataAnnotations;

namespace MVC.Models.Warehouses
{
    public class WarehouseFormModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "اسم المخزن مطلوب.")]
        [StringLength(120, ErrorMessage = "اسم المخزن لا يتجاوز 120 حرفاً.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "نوع المخزن مطلوب.")]
        public int Type { get; set; }

        [Required(ErrorMessage = "الموقع مطلوب.")]
        [StringLength(250, ErrorMessage = "الموقع لا يتجاوز 250 حرفاً.")]
        public string Location { get; set; } = null!;

        [StringLength(500, ErrorMessage = "الوصف لا يتجاوز 500 حرفاً.")]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        public List<BranchDto> Branches { get; set; } = new();
        public List<Guid> SelectedBranchIds { get; set; } = new();
    }

    public class WarehouseIndexModel
    {
        public List<WarehouseDto> Warehouses { get; set; } = new();
        public Dictionary<Guid, WarehouseSummaryModel> Summaries { get; set; } = new();
        public IDictionary<Guid, List<string>> BranchNames { get; set; } = new Dictionary<Guid, List<string>>();
    }

    public class WarehouseSummaryModel
    {
        public int ProductCount { get; set; }
        public int TotalUnits { get; set; }
    }

    public class WarehouseStockModel
    {
        public WarehouseDto Warehouse { get; set; } = new();
        public List<ProductStockByWarehouseDto> Items { get; set; } = new();
        public int ProductCount { get; set; }
        public int TotalUnits { get; set; }
    }

    public class TransferStockModel
    {
        public List<WarehouseDto> Warehouses { get; set; } = new();
        public List<ProductDto> Products { get; set; } = new();

        [Required(ErrorMessage = "اختر المنتج.")]
        public Guid ProductId { get; set; }

        [Required(ErrorMessage = "اختر مخزن التحويل (المصدر).")]
        public Guid FromWarehouseId { get; set; }

        [Required(ErrorMessage = "اختر المخزن المستهدف.")]
        public Guid ToWarehouseId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "الكمية يجب أن تكون أكبر من صفر.")]
        public int Quantity { get; set; }

        [StringLength(500, ErrorMessage = "الملاحظات لا تتجاوز 500 حرفاً.")]
        public string? Notes { get; set; }
    }

    public static class WarehouseTypeLabels
    {
        public const int Shop = 0;
        public const int Storage = 1;

        public static string Label(int type) => type switch
        {
            Shop => "بيع (رفوف المحل)",
            Storage => "تخزين (مخزن رئيسي)",
            _ => "غير محدد"
        };

        public static string BadgeClass(int type) => type switch
        {
            Shop => "bg-success-subtle text-success",
            Storage => "bg-info-subtle text-primary",
            _ => "bg-light text-dark"
        };

        public static string Label(string type) => Label(Parse(type));

        public static string BadgeClass(string type) => BadgeClass(Parse(type));

        public static int Parse(string type) =>
            string.Equals(type, "Shop", StringComparison.OrdinalIgnoreCase) ? Shop : Storage;
    }
}