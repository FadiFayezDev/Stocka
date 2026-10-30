using System;

namespace Identity.Infrastructure.Exceptions
{
    /// <summary>Requested value was not found.</summary>
    public class NotFoundException : Exception
    {
        public NotFoundException(string message)
            : base(message)
        {
        }
    }
}
