using Application.Dtos.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos.ReceiveAndStockPurchasedProducts
{
    public class WarehouseDistributionDto
    {
        public Guid WarehouseId { get; set; }
        public int Quantity { get; set; }
    }
}