using Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public virtual ICollection<UserPhoneNumber> PhoneNumbers { get; set; } = new List<UserPhoneNumber>();
        public virtual ICollection<BrandMembership> BrandMemberships { get; set; } = new List<BrandMembership>();
    }
}