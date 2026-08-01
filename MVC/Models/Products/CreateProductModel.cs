using Application.Features.Commands.Product.Create;

namespace MVC.Models.Products
{
    public class CreateProductModel
    {
        public CreateProductCommand Command { get; set; }
        public Dictionary<Guid, string> Categories { get; set; }
    }
}
