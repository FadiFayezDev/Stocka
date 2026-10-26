using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.DTOs.Brand
{
    public record BrandDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Slug { get; set; } = default!;
    }
}
