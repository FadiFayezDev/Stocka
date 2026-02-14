using Application.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Interfaces
{
    public interface ITokenGenerator
    {
        //public string GenerateToken(string userName, string password);
        public string GenerateJWTToken(UserTokenDetailsDto userDetails);
    }
}
