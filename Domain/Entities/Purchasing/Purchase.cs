using Domain.Bases;
using Domain.Entities.Core;
using Domain.Entities.Products;
using Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Domain.Entities.Purchasing
{
    public partial class Purchase : AggregateRoot<PurchaseId>, IMultiTenantEntity, IBranchScopedEntity
    {
        public BrandId BrandId { get; private set; }
        public BranchId? BranchId { get; private set; }

        public SupplierId SupplierId { get; private set; }

        public DateTime PurchaseDate { get; private set; }

        public decimal TotalAmount { get; private set; }

        private readonly List<PurchaseItem> _purchaseItems = new();

        public virtual ICollection<PurchaseItem> PurchaseItems => _purchaseItems.AsReadOnly();

        private Purchase() { }

        [SetsRequiredMembers]
        public Purchase(BrandId brandId, SupplierId supplierId, DateTime? purchaseDate = null, BranchId? branchId = null)
        {
            Id = PurchaseId.New();

            BrandId = brandId;
            BranchId = branchId;
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

        public PurchaseItem AddPurchaseItem(ProductId productId, int quantity, decimal unitCost)
        {
            var item = new PurchaseItem(Id, productId, quantity, unitCost);
            AddPurchaseItem(item);
            return item;
        }

        public void UpdatePurchaseDate(DateTime newDate)
        {
            if (newDate > DateTime.UtcNow)
                throw new ArgumentException("Purchase date cannot be in the future.", nameof(newDate));

            PurchaseDate = newDate;
        }

        public void RemovePurchaseItem(PurchaseItemId itemId)
        {
            var item = _purchaseItems.FirstOrDefault(pi => pi.Id == itemId);
            if (item == null)
                throw new ArgumentException("Purchase item not found.");

            _purchaseItems.Remove(item);
            RecalculateTotal();
        }

        public void UpdatePurchaseItem(PurchaseItemId itemId, int quantity, decimal unitCost)
        {
            var item = _purchaseItems.FirstOrDefault(pi => pi.Id == itemId);
            if (item == null)
                throw new ArgumentException("Purchase item not found.");

            item.UpdateQuantity(quantity);
            item.UpdateUnitCost(unitCost);
            RecalculateTotal();
        }

        private void RecalculateTotal()
        {
            TotalAmount = _purchaseItems.Sum(pi => pi.TotalCost);
        }

        Guid IMultiTenantEntity.BrandId
        {
            get => BrandId.Value;
            set => BrandId = new BrandId(value);
        }

        Guid IBranchScopedEntity.BranchId
        {
            get => BranchId?.Value ?? Guid.Empty;
            set => BranchId = value == Guid.Empty ? null : new BranchId(value);
        }
    }
}
