using System;

namespace Application.Dtos.Core
{
    public class PhoneNumberDto
    {
        public Guid UserId { get; set; }
        public string Number { get; set; } = null!;
    }
}
