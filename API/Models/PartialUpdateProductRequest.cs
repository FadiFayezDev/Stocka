namespace API.Models
{
    public class PartialUpdateProductRequest
    {
        public Guid Id { get; set; }
        public Guid? CategoryId { get; set; }
        public string? Name { get; set; }
        public decimal? SellingPrice { get; set; }
        public string? Barcode { get; set; }
        public IFormFile? Image { get; set; }
        public bool? IsActive { get; set; }
    }
}
