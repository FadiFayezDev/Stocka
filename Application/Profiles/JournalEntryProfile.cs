using Application.Dtos.Accounting;
using Application.UseCases.JournalEntryCases;
using AutoMapper;
using Domain.Entities.Accounting;

namespace Application.Profiles
{
    public class JournalEntryProfile : Profile
    {
        public JournalEntryProfile()
        {
            // Entity ? DTO
            CreateMap<JournalEntry, JournalEntryDto>().ReverseMap();

            // Command ? Entity
            CreateMap<RecordJournalEntryCommand, JournalEntry>();
            CreateMap<UpdateJournalEntryCommand, JournalEntry>();
        }
    }
}
