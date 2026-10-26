using FluentValidation;
using Inventory.Contracts.UseCases.StockCases;

namespace Inventory.Application.Validators.StockCases;

#region Validator
/// <summary>
/// Validates the command so an allocation always references concrete identifiers and a positive quantity.
/// </summary>
public class AllocateStockCommandValidator : AbstractValidator<AllocateStockCommand>
{
    public AllocateStockCommandValidator()
    {
        RuleFor(x => x.BatchId).NotEqual(Guid.Empty);
        RuleFor(x => x.WarehouseId).NotEqual(Guid.Empty);
        RuleFor(x => x.Quantity).GreaterThan(0);
    }
}
#endregion