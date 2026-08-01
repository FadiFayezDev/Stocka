using Domain.Bases;
using Domain.Entities.Core;
using Domain.Entities.Orders;
using Domain.Entities.Purchasing;
using Domain.Primitives;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities.Products
{
    public partial class Product : AggregateRoot<ProductId>, IMultiTenantEntity
    {
        private readonly List<Batch> _batches = new();
        private readonly List<PurchaseItem> _purchaseItems = new();
        private readonly List<OrderItem> _orderItems = new();
        private readonly List<StockMovement> _stockMovements = new();

        private Product()
        {
            // Required by EF Core
        }

        [SetsRequiredMembers]
        private Product(
            BrandId brandId,
            ProductCategoryId categoryId,
            string name,
            decimal sellingPrice,
            string? barcode)
        {
            Id = ProductId.New();

            ChangeBrand(brandId);
            ChangeCategory(categoryId);
            Rename(name);
            ChangeSellingPrice(sellingPrice);
            ChangeBarcode(barcode);

            IsActive = true;
        }

        #region Properties

        public BrandId BrandId { get; private set; } = default!;

        public ProductCategoryId CategoryId { get; private set; } = default!;

        public string Name { get; private set; } = string.Empty;

        public decimal SellingPrice { get; private set; }

        public string? Barcode { get; private set; }

        public string? ImagePath { get; private set; }

        public bool IsActive { get; private set; }

        #endregion

        #region Navigation Properties

        public Brand Brand { get; private set; } = null!;

        public ProductCategory Category { get; private set; } = null!;

        public IReadOnlyCollection<Batch> Batches => _batches;

        public IReadOnlyCollection<PurchaseItem> PurchaseItems => _purchaseItems;

        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

        public IReadOnlyCollection<StockMovement> StockMovements => _stockMovements;

        Guid IMultiTenantEntity.BrandId { get => BrandId.Value; set => BrandId = new BrandId(value); }

        #endregion

        #region Factory

        public static Product Create(
            BrandId brandId,
            ProductCategoryId categoryId,
            string name,
            decimal sellingPrice,
            string? barcode = null)
        {
            return new Product(
                brandId,
                categoryId,
                name,
                sellingPrice,
                barcode);
        }

        #endregion

        #region Business Methods

        public void Rename(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Product name cannot be empty.", nameof(name));

            Name = name.Trim();
        }

        public void ChangeSellingPrice(decimal sellingPrice)
        {
            if (sellingPrice < 0)
                throw new ArgumentOutOfRangeException(nameof(sellingPrice));

            SellingPrice = sellingPrice;
        }

        public void ChangeBrand(BrandId brandId)
        {
            BrandId = brandId;
        }

        public void ChangeCategory(ProductCategoryId categoryId)
        {
            CategoryId = categoryId;
        }

        public void ChangeBarcode(string? barcode)
        {
            Barcode = string.IsNullOrWhiteSpace(barcode)
                ? null
                : barcode.Trim();
        }

        public void ChangeImage(string? imagePath)
        {
            ImagePath = string.IsNullOrWhiteSpace(imagePath)
                ? null
                : imagePath.Trim();
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        #endregion

        #region Batches

        public void AddBatch(Batch batch)
        {
            ArgumentNullException.ThrowIfNull(batch);

            if (batch.ProductId != Id)
                throw new InvalidOperationException("Batch belongs to another product.");

            if (_batches.Any(x => x.Id == batch.Id))
                throw new InvalidOperationException("Batch already exists.");

            _batches.Add(batch);
        }

        public void RemoveBatch(Batch batch)
        {
            ArgumentNullException.ThrowIfNull(batch);

            _batches.Remove(batch);
        }

        #endregion
    }
}