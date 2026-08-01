using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Exceptions
{
    public class PersistenceOperationException : Exception
    {

        public PersistenceOperationException(string message, Exception? inner = null) : base(message, inner) 
        {

        }
    }
}
