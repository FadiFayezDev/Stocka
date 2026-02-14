using System;

namespace Application.Dtos.Accounting
{
    public class AccountDto
    {
        public Guid Id { get; set; }
        public Guid BrandId { get; set; }
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!;
    }
}
