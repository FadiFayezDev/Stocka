using Application.Dtos.Expenses;
using Domain.Entities.Expenses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.QueryRepositories
{
    public interface IExpenseCategoryQueryRepository
    {
        Task<ExpenseCategoryDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<ExpenseCategoryDto>> GetAllTableAsync();
        Task<IEnumerable<ExpenseCategoryDto>> GetAllByBrandIdAsync(Guid brandId);
    }
}
