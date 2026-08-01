namespace API.Models
{
    public class CreateProductRequest
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; } = null!;
        public decimal SellingPrice { get; set; }
        public string? Barcode { get; set; }
        public IFormFile? Image { get; set; }
    }
}