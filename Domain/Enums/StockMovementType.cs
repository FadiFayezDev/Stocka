using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Enums
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
