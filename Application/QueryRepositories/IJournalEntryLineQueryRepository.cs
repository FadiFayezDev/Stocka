using Application.Dtos.Accounting;
using Domain.Entities.Accounting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.QueryRepositories
{
    public interface IJournalEntryLineQueryRepository
    {
        Task<JournalEntryLineDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<JournalEntryLineDto>> GetAllTableAsync();
        Task<IEnumerable<JournalEntryLineDto>> GetAllByBrandIdAsync(Guid brandId);
    }
}
