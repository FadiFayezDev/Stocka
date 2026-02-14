using Domain.Bases;
using Domain.Entities.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.Core
{
    // Domain/Entities/Employee.cs
    public class Employee : IEntity<Guid>
    {
        public Guid GetKey() => Id;

        public void SetKey(Guid key) => Id = key;

        public Guid Id { get; set; }

        public Guid? UserId { get; set; }
        public Guid BrandId { get; set; }

        public string JobTitle { get; set; } = null!;
        public decimal? Salary { get; set => field = value > 0 ? value : throw new ArgumentException("SALARY_CANNOT_BE_ZERO."); }
        public DateTime HireDate { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; }

        public virtual Brand Brand { get; set; }
    }
}