using Application.Dtos.Core;
using Domain.Enums;

namespace Application.DTOs
{
    public class AuthResponseDTO
    {
        public string Name { get; set; } = string.Empty; 
        public string Token { get; set; } = string.Empty;
        public List<BrandShortDto> Brands { get; set; } = new();
    }
}