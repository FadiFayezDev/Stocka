using Domain.Bases;
using Domain.Entities.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.Core
{
    public class Employee : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public Guid BrandId { get; set; }
        public string JobTitle { get; set; } = null!;
        public decimal? Salary { get; set; }
        public DateTime HireDate { get; set; }
        public bool IsActive { get; private set; }

        private Employee() { }

        public Employee(Guid? userId, Guid brandId, string jobTitle, decimal? salary, DateTime? hireDate = null)
        {
            Id = Guid.NewGuid();

            if (string.IsNullOrWhiteSpace(jobTitle))
                throw new ArgumentException("Job title cannot be empty.", nameof(jobTitle));

            if (salary.HasValue && salary.Value <= 0)
                throw new ArgumentException("Salary must be greater than zero.", nameof(salary));

            UserId = userId;
            BrandId = brandId;
            JobTitle = jobTitle.Trim();
            Salary = salary;
            HireDate = hireDate ?? DateTime.UtcNow;
            IsActive = true;
        }

        public void UpdateJobTitle(string newJobTitle)
        {
            if (string.IsNullOrWhiteSpace(newJobTitle))
                throw new ArgumentException("Job title cannot be empty.", nameof(newJobTitle));

            JobTitle = newJobTitle.Trim();
        }

        public void UpdateSalary(decimal newSalary)
        {
            if (newSalary <= 0)
                throw new ArgumentException("Salary must be greater than zero.", nameof(newSalary));

            Salary = newSalary;
        }

        public void Deactivate()
        {
            if (!IsActive)
                throw new InvalidOperationException("Employee is already inactive.");

            IsActive = false;
        }

        public void Activate()
        {
            if (IsActive)
                throw new InvalidOperationException("Employee is already active.");

            IsActive = true;
        }

        public Guid GetKey() => Id;

        public void SetKey(Guid key) => Id = key;
    }
}