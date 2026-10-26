using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Domain.Enums
{
    public enum StockMovementType
    {
        PurchaseIn,

        SaleOut,

        TransferIn,
        TransferOut,

        AdjustmentIn,
        AdjustmentOut,

        OrderReturn
    }
}
