using Application.Dtos.Accounting;
using Domain.Entities.Accounting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.QueryRepositories
{
    public interface IJournalEntryQueryRepository
    {
        Task<JournalEntryDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<JournalEntryDto>> GetAllTableAsync();
        Task<IEnumerable<JournalEntryDto>> GetAllByBrandIdAsync(Guid brandId);
    }
}
