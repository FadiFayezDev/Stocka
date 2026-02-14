using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Identity
{
    public class UserPhoneNumber
    {
        public Guid Id { get; set; }
        public Guid ApplicationUserId { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public bool IsPrimary { get; set; }

        public virtual ApplicationUser User { get; set; } = null!;
    }
}