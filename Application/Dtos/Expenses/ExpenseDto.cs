using System;

namespace Application.Dtos.Expenses
{
    public class ExpenseDto
    {
        public Guid Id { get; set; }
        public Guid BrandId { get; set; }
        public Guid CategoryId { get; set; }
        public decimal Amount { get; set; }
        public DateTime ExpenseDate { get; set; }
    }
}
