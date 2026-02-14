namespace Application.Dtos.Auth
{
    // 2. DTO مختصر لقائمة المستخدمين (لـ Grid أو Table)
    public record UserSummaryDto(
        Guid Id,
        string FirstName,
        string LastName,
        string UserName,
        string Email
    );
}
