using Application.Dtos.Accounting;
using Application.Features.Commands.JournalEntryLine;
using Application.Features.Commands.JournalEntryLine.Create;
using Application.Features.Commands.JournalEntryLine.Update;
using AutoMapper;
using Domain.Entities.Accounting;

namespace Application.Profiles
{
    public class JournalEntryLineProfile : Profile
    {
        public JournalEntryLineProfile()
        {
            // Entity ? DTO
            CreateMap<JournalEntryLine, JournalEntryLineDto>().ReverseMap();

            // Command ? Entity
            CreateMap<CreateJournalEntryLineCommand, JournalEntryLine>();
            CreateMap<UpdateJournalEntryLineCommand, JournalEntryLine>();
        }
    }
}
