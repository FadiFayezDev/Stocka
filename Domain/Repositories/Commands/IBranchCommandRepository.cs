using Domain.Entities.Core;
using Domain.Repositories.Commands.Base;

namespace Domain.Repositories.Commands
{
    public interface IBranchCommandRepository : ICommandRepository<Branch>
    {
    }
}
