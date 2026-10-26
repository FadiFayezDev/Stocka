using FluentValidation;
using Inventory.Contracts.UseCases.WarehouseCases;

namespace Inventory.Application.Validators.WarehouseCases;

#region Validator
/// <summary>
/// Validates the ActivateWarehouseCommand to ensure required properties meet defined rules.
/// </summary>
public class ActivateWarehouseCommandValidator : AbstractValidator<ActivateWarehouseCommand>
{
    public ActivateWarehouseCommandValidator() => RuleFor(x => x.WarehouseId).NotEqual(Guid.Empty);
}
#endregion