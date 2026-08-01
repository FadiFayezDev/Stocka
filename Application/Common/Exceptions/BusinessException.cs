using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Exceptions
{
    public class BusinessException : Exception
    {
        public string Code { get; set; }

        public BusinessException(string code) : base(code)
        {
            Code = code;
        }
    }
}