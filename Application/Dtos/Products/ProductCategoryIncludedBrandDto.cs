using Application.Dtos.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos.Products
{
    public class ProductCategoryIncludedBrandDto
    {
        public Guid Id { get; set; }
        public Guid BrandId { get; set; }
        public string Name { get; set; } = null!;

        public BrandDto Brand { get; set; } =  null!;
    }
}
