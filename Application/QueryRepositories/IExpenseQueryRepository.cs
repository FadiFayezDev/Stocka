using Application.Dtos.Expenses;
using Domain.Entities.Expenses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.QueryRepositories
{
    public interface IExpenseQueryRepository
    {
        Task<ExpenseDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<ExpenseDto>> GetAllTableAsync();
        Task<IEnumerable<ExpenseDto>> GetAllByBrandIdAsync(Guid brandId);
    }
}
