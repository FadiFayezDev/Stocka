using FluentValidation;
using Inventory.Contracts.UseCases.BatchCases;

namespace Inventory.Application.Validators.BatchCases;

#region Validator
/// <summary>
/// Validates the command so a receipt always references concrete product/purchase-item
/// identifiers and positive quantity and unit cost.
/// </summary>
public class RegisterBatchCommandValidator : AbstractValidator<RegisterBatchCommand>
{
    public RegisterBatchCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEqual(Guid.Empty).WithMessage("Product reference is required.");
        RuleFor(x => x.PurchaseItemId).NotEqual(Guid.Empty).WithMessage("Purchase item reference is required.");
        RuleFor(x => x.InitialQuantity).GreaterThan(0);
        RuleFor(x => x.UnitCost).GreaterThan(0);
    }
}
#endregion