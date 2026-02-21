namespace API.Models
{
    public class CreateProductRequest
    {
        public Guid BrandId { get; set; }
        public Guid CategoryId { get; set; }
        public string Name { get; set; } = null!;
        public string? Barcode { get; set; }
        public IFormFile? Image { get; set; }
    }
}