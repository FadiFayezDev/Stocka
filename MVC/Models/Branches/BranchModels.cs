using Application.Dtos.Core;
using Application.Dtos.Products;
using System.ComponentModel.DataAnnotations;

namespace MVC.Models.Branches
{
    public class BranchFormModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "اسم الفرع مطلوب.")]
        [StringLength(200, ErrorMessage = "اسم الفرع لا يتجاوز 200 حرفاً.")]
        public string Name { get; set; } = null!;

        public List<WarehouseDto> Warehouses { get; set; } = new();
        public List<Guid> SelectedWarehouseIds { get; set; } = new();
    }

    public class BranchIndexModel
    {
        public List<BranchDto> Branches { get; set; } = new();
        public IDictionary<Guid, List<string>> WarehouseNames { get; set; } = new Dictionary<Guid, List<string>>();
    }
}