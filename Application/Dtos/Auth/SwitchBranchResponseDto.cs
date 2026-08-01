using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos.Auth
{
    public class SwitchBranchResponseDto
    {
        public string Token { get; set; } = null!;

        public SwitchBranchResponseDto(string token) 
        { 
            Token = token;
        }
    }
}
