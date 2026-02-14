using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Application.DTOs
{
    public class AuthResponseDTO
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public List<Guid> BrandIds { get; set; }
        public string Token { get; set; }
    }
}
