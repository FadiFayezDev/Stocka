using System;

namespace Identity.Contracts.DTOs
{
    /// <summary>
    /// A Supplier (company/business role) of a Party. The shared primary key
    /// (<c>Id</c>) is the Party's id; the contact person's data (names/phone)
    /// comes from the joined Party row.
    /// </summary>
    public record SupplierDto(
        Guid Id,
        string Name,
        string? Email,
        string? Address,
        string FirstName,
        string LastName,
        string PhoneNumber
    );
}
