using Domain.Entities.Products;
using Domain.Repositories.Commands.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Repositories.Commands
{
    public interface IProductCommandRepository : ICommandRepository<Product>
    {
    }
}
