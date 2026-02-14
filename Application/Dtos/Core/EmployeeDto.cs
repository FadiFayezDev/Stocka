using System;

namespace Application.Dtos.Core
{
    public class EmployeeDto
    {
        public Guid Id { get; set; }
        public Guid ApplicationUserId { get; set; }
        public Guid BrandId { get; set; }
        public string JobTitle { get; set; } = null!;
        public decimal? Salary { get; set; }
        public DateTime HireDate { get; set; }
        public bool IsActive { get; set; }
    }
}
