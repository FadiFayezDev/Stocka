using System;

namespace Application.Dtos.Core
{
    public class CustomerDto
    {
        public Guid Id { get; set; }
        public Guid ApplicationUserId { get; set; }
        public Guid? BrandId { get; set; }
        public int LoyaltyPoints { get; set; }
    }
}
