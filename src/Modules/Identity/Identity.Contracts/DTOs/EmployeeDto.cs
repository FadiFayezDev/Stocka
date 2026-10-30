using System;

namespace Identity.Contracts.DTOs
{
    /// <summary>
    /// An Employee role of a Party. The shared primary key
    /// (<c>Id</c>) is the Party's id; the person's data (names/phone)
    /// comes from the joined Party row.
    /// </summary>
    public record EmployeeDto(
        Guid Id,
        string JobTitle,
        string FirstName,
        string LastName,
        string PhoneNumber
    );
}
