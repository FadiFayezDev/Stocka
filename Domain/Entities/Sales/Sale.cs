using Domain.Bases;
using Domain.Entities.Core;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Net.ServerSentEvents;
using System.Text;

namespace Domain.Entities.Sales
{
    public partial class Sale : IEntity<Guid>
    {
        public Guid GetKey() => Id;

        public void SetKey(Guid key) => Id = key;

        public Guid Id { get; set; }

        public Guid BrandId { get; set; }

        public Guid EmployeeId { get; set; }

        public Guid? CustomerId { get; set; }

        public DateTime SaleDate { get; set; }

        public SaleStatus Status { get; set; }

        public decimal TotalAmount { get; set; }

        public virtual Brand Brand { get; set; } = null!;

        public virtual Employee? Employee { get; set; }

        public virtual Customer? Customer { get; set; }

        public virtual ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
    }
}