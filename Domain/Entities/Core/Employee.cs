using Domain.Bases;
using Domain.Entities.Core;
using Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Domain.Entities.Core
{
    public class Employee : AggregateRoot<EmployeeId>, IMultiTenantEntity, IBranchScopedEntity
    {
        public Guid? UserId { get; private set; }
        public BrandId BrandId { get; private set; }
        public BranchId BranchId { get; private set; }
        public string JobTitle { get; private set; } = null!;
        public decimal? Salary { get; private set; }
        public DateTime HireDate { get; private set; }
        public bool IsActive { get; private set; }

        private Employee() { }

        [SetsRequiredMembers]
        public Employee(Guid? userId, BrandId brandId, string jobTitle, decimal? salary, BranchId branchId, DateTime? hireDate = null)
        {
            Id = EmployeeId.New();

            if (string.IsNullOrWhiteSpace(jobTitle))
                throw new ArgumentException("Job title cannot be empty.", nameof(jobTitle));

            if (salary.HasValue && salary.Value <= 0)
                throw new ArgumentException("Salary must be greater than zero.", nameof(salary));

            UserId = userId;
            BrandId = brandId;
            BranchId = branchId;
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

        public void UpdateHireDate(DateTime newDate)
        {
            if (newDate > DateTime.UtcNow)
                throw new ArgumentException("Hire date cannot be in the future.", nameof(newDate));

            HireDate = newDate;
        }

        public void AssignToBranch(BranchId branchId)
        {
            BranchId = branchId;
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

        Guid IMultiTenantEntity.BrandId
        {
            get => BrandId.Value;
            set => BrandId = new BrandId(value);
        }

        Guid IBranchScopedEntity.BranchId
        {
            get => BranchId.Value;
            set => BranchId = new BranchId(value);
        }
    }
}
