using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Exceptions
{
    public class IdentityOperationException : Exception
    {
        public IdentityOperationException(IEnumerable<IdentityError> errors)
            : base(string.Join(", ", errors.Select(e => e.Description)))
        {
        }
    }
}
