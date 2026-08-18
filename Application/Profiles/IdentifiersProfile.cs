using AutoMapper;
using Domain.Primitives;

namespace Application.Profiles
{
    public class IdentifiersProfile : Profile
    {
        public IdentifiersProfile()
        {
            MapId<AccountId>();
            MapId<BatchId>();
            MapId<BranchId>();
            MapId<BrandId>();
            MapId<CustomerId>();
            MapId<EmployeeId>();
            MapId<ExpenseCategoryId>();
            MapId<ExpenseId>();
            MapId<JournalEntryId>();
            MapId<JournalEntryLineId>();
            MapId<OrderId>();
            MapId<OrderItemId>();
            MapId<ProductCategoryId>();
            MapId<ProductId>();
            MapId<PurchaseId>();
            MapId<PurchaseItemId>();
            MapId<StockMovementId>();
            MapId<SupplierId>();
            MapId<UserId>();
            MapId<WarehouseBatchId>();
            MapId<WarehouseBranchId>();
            MapId<WarehouseId>();
        }

        private void MapId<TId>() where TId : struct
        {
            CreateMap<TId, Guid>().ConvertUsing(id => IdExpressions<TId>.GetValue(id));

            CreateMap<Guid, TId>().ConvertUsing(value =>
                value == Guid.Empty ? default : IdExpressions<TId>.Create(value));
        }
    }

    internal static class IdExpressions<TId> where TId : struct
    {
        public static readonly Func<TId, Guid> GetValue =
            BuildGetter();

        public static readonly Func<Guid, TId> Create =
            BuildCreator();

        private static Func<TId, Guid> BuildGetter()
        {
            var parameter = System.Linq.Expressions.Expression.Parameter(typeof(TId), "id");
            var body = System.Linq.Expressions.Expression.Property(parameter, "Value");
            return System.Linq.Expressions.Expression
                .Lambda<Func<TId, Guid>>(body, parameter)
                .Compile();
        }

        private static Func<Guid, TId> BuildCreator()
        {
            var parameter = System.Linq.Expressions.Expression.Parameter(typeof(Guid), "value");
            var constructor = typeof(TId).GetConstructor(new[] { typeof(Guid) });
            var body = System.Linq.Expressions.Expression.New(constructor!, parameter);
            return System.Linq.Expressions.Expression
                .Lambda<Func<Guid, TId>>(body, parameter)
                .Compile();
        }
    }
}
