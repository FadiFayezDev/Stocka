using Domain.Entities.Expenses;
using Domain.Repositories.Commands.Base;

namespace Domain.Repositories.Commands
{
    public interface IExpenseCommandRepository : ICommandRepository<Expense>
    {
    }
}
