using System;

namespace Application.Dtos.Expenses
{
    public class ExpenseCategoryDto
    {
        public Guid Id { get; set; }
        public Guid BrandId { get; set; }
        public string Name { get; set; } = null!;
    }
}
