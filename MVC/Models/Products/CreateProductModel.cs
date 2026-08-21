namespace MVC.Models.Products
{
    public class CreateProductModel
    {
        public Guid CategoryId { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal SellingPrice { get; set; }

        public string? Barcode { get; set; }

        public IFormFile? ImageFile { get; set; }

        public Dictionary<Guid, string> Categories { get; set; } = new();
    }
}