using Application.Dtos.Accounting;
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
        }
    }
}
