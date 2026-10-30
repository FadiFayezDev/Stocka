using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Identity.Infrastructure.Exceptions
{
    /// <summary>
    /// Aggregated failure of one or more ASP.NET Identity operations; the message
    /// joins all IdentityError descriptions so callers see every reason at once.
    /// </summary>
    public class IdentityOperationException : Exception
    {
        public IdentityOperationException(IEnumerable<IdentityError> errors)
            : base(string.Join(", ", errors.Select(e => e.Description)))
        {
        }

        public IdentityOperationException(string message)
            : base(message)
        {
        }
    }
}
