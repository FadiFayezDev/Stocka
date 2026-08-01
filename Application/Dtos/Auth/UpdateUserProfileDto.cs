namespace Application.Dtos.Auth
{
    // 5. DTO لتحديث الملف الشخصي
    public record UpdateUserProfileDto(
        string Id,
        string FirstName,
        string LastName,
        string Email,
        IList<string> Roles
    );
}
