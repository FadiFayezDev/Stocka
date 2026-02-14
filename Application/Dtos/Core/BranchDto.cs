using System;

namespace Application.Dtos.Core
{
    public class BranchDto
    {
        public Guid Id { get; set; }
        public Guid BrandId { get; set; }
        public string Name { get; set; } = null!;
    }
}
