using Domain.Bases;
using Domain.Entities.Core;
using Domain.Entities.Purchasing;
using Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.Products
{
    public partial class Product : IEntity<Guid>
    {
        public Guid Id { get; set; }

        public Guid BrandId { get; set; }

        public Guid CategoryId { get; set; }

        public string Name { get; set; } = null!;

        public string? Barcode { get; set; }

        private readonly List<Batch> _batches = new();
        private readonly List<PurchaseItem> _purchaseItems = new();
        private readonly List<OrderItem> _OrderItems = new();
        private readonly List<StockMovement> _stockMovements = new();

        public virtual ICollection<Batch> Batches => _batches.AsReadOnly();
        public virtual Brand Brand { get; set; } = null!;
        public virtual ProductCategory Category { get; set; } = null!;
        public virtual ICollection<PurchaseItem> PurchaseItems => _purchaseItems.AsReadOnly();
        public virtual ICollection<OrderItem> OrderItems => _OrderItems.AsReadOnly();
        public virtual ICollection<StockMovement> StockMovements => _stockMovements.AsReadOnly();

        private Product() { }

        public Product(Guid brandId, Guid categoryId, string name, string? barcode = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Product name cannot be empty.", nameof(name));

            BrandId = brandId;
            CategoryId = categoryId;
            Name = name.Trim();
            Barcode = barcode?.Trim();
        }

        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Product name cannot be empty.", nameof(newName));

            Name = newName.Trim();
        }

        public void UpdateBarcode(string? newBarcode)
        {
            Barcode = newBarcode?.Trim();
        }

        public void AddBatch(Batch batch)
        {
            if (batch == null)
                throw new ArgumentNullException(nameof(batch));

            if (batch.ProductId != Id)
                throw new ArgumentException("Batch does not belong to this product.");

            if (_batches.Any(b => b.Id == batch.Id))
                throw new InvalidOperationException("Batch already added.");

            _batches.Add(batch);
        }

        public Guid GetKey() => Id;

        public void SetKey(Guid key) => Id = key;
    }
}
