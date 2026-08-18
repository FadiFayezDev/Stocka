using Application.UseCases.ProductCases;

namespace MVC.Models.Products
{
    public class CreateProductModel
    {
        public RegisterProductCommand Command { get; set; }
        public Dictionary<Guid, string> Categories { get; set; }
    }
}
