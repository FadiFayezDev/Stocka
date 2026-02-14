using System;

namespace Application.Dtos.Accounting
{
    public class JournalEntryDto
    {
        public Guid Id { get; set; }
        public Guid BrandId { get; set; }
        public DateTime EntryDate { get; set; }
        public string? Description { get; set; }
    }
}
