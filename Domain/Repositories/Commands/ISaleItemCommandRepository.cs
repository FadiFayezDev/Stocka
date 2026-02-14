using Domain.Entities.Products;
using Domain.Entities.Sales;
using Domain.Repositories.Commands.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Repositories.Commands
{
    public interface ISaleItemCommandRepository : ICommandRepository<SaleItem>
    {
    }
}
