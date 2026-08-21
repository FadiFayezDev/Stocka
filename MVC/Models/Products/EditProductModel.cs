namespace MVC.Models.Products
{
    public class EditProductModel
    {
        public Guid Id { get; set; }

        public Guid CategoryId { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal SellingPrice { get; set; }

        public string? Barcode { get; set; }

        public bool IsActive { get; set; }

        public string? ImageUrl { get; set; }

        public IFormFile? Image { get; set; }

        public Dictionary<Guid, string> Categories { get; set; } = new();
    }
}