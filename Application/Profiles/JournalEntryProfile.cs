using Application.Dtos.Accounting;
using Application.Features.Commands.JournalEntry;
using Application.Features.Commands.JournalEntry.Create;
using Application.Features.Commands.JournalEntry.Update;
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
            CreateMap<CreateJournalEntryCommand, JournalEntry>();
            CreateMap<UpdateJournalEntryCommand, JournalEntry>();
        }
    }
}
