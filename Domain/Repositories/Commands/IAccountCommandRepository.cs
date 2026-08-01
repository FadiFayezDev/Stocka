using Domain.Entities.Accounting;
using Domain.Repositories.Commands.Base;

namespace Domain.Contracts
{
    public interface IAccountCommandRepository : ICommandRepository<Account>
    {
    }
}
