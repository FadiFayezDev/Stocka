using Application.Dtos.Products;
using MediatR;

namespace API.Models
{
    public class UpdateProductRequest
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public string Name { get; set; } = null!;
        public decimal SellingPrice { get; set; }
        public string? Barcode { get; set; }
        public IFormFile? Image { get; set; }
    }
}