using Domain.Entities.Core;
using Domain.Repositories.Commands.Base;

namespace Domain.Contracts
{
    public interface ICustomerCommandRepository : ICommandRepository<Customer>
    {
    }
}
