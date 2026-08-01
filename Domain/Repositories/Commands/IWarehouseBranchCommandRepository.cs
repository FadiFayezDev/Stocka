using Domain.Repositories.Commands.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Domain.Repositories.Commands
{
    public interface IWarehouseBranchCommandRepository : ICommandRepository<Entities.Products.WarehouseBranch>
    {
    }
}
