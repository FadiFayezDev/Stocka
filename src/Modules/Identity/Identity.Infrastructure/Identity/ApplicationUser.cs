using Identity.Domain.Parties;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public PartyId PartyId { get; set; }
    }
}
