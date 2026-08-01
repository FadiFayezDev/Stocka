namespace Application.Dtos.Auth
{
    // 3. DTO لإنشاء مستخدم جديد
    public record CreateUserDto(
        string FirstName,
        string LastName,
        string UserName,
        string Password,
        string Email,
        List<string>? Roles = null
    );
}
