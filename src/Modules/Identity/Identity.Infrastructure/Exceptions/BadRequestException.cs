using System;

namespace Identity.Infrastructure.Exceptions
{
    /// <summary>Caller requested an operation that the business rules forbid.</summary>
    public class BadRequestException : Exception
    {
        public BadRequestException(string message)
            : base(message)
        {
        }
    }
}
