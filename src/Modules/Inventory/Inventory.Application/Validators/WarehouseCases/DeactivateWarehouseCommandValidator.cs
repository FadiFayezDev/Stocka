using FluentValidation;
using Inventory.Contracts.UseCases.WarehouseCases;

namespace Inventory.Application.Validators.WarehouseCases;

#region Validator
/// <summary>
/// Validates the DeactivateWarehouseCommand to ensure required properties meet defined rules.
/// </summary>
public class DeactivateWarehouseCommandValidator : AbstractValidator<DeactivateWarehouseCommand>
{
    public DeactivateWarehouseCommandValidator() => RuleFor(x => x.WarehouseId).NotEqual(Guid.Empty);
}
#endregion