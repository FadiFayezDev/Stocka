using Domain.Bases;
using Domain.Entities.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.Purchasing
{
    public partial class Purchase : IEntity<Guid>
    {
        public Guid Id { get; set; }

        public Guid BrandId { get; set; }

        public Guid SupplierId { get; set; }

        public DateTime PurchaseDate { get; set; }

        public decimal TotalAmount { get; private set; }

        private readonly List<PurchaseItem> _purchaseItems = new();

        public virtual Brand Brand { get; set; } = null!;
        public virtual ICollection<PurchaseItem> PurchaseItems => _purchaseItems.AsReadOnly();
        public virtual Supplier Supplier { get; set; } = null!;

        private Purchase() { }

        public Purchase(Guid brandId, Guid supplierId, DateTime? purchaseDate = null)
        {
            Id = Guid.NewGuid();

            BrandId = brandId;
            SupplierId = supplierId;
            PurchaseDate = purchaseDate ?? DateTime.UtcNow;
            TotalAmount = 0;
        }

        public void AddPurchaseItem(PurchaseItem item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            if (item.PurchaseId != Id)
                throw new ArgumentException("Purchase item does not belong to this purchase.");

            if (_purchaseItems.Any(pi => pi.Id == item.Id))
                throw new InvalidOperationException("Purchase item already added.");

            _purchaseItems.Add(item);
            RecalculateTotal();
        }

        public void RemovePurchaseItem(Guid itemId)
        {
            var item = _purchaseItems.FirstOrDefault(pi => pi.Id == itemId);
            if (item == null)
                throw new ArgumentException("Purchase item not found.");

            _purchaseItems.Remove(item);
            RecalculateTotal();
        }

        private void RecalculateTotal()
        {
            TotalAmount = _purchaseItems.Sum(pi => pi.TotalCost);
        }

        public Guid GetKey() => Id;

        public void SetKey(Guid key) => Id = key;
    }
}
