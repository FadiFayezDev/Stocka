using Application.Dtos.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class AuthResponseDTO
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public List<BrandDto> Brands { get; set; }
        public string Token { get; set; }
    }
}