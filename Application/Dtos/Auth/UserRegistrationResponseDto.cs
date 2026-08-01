namespace Application.Dtos.Auth
{
    // 4. DTO لرد عملية الإنشاء (بدل الـ Tuple)
    public record UserRegistrationResponseDto(
        bool IsSucceed,
        Guid UserId
    );
}
