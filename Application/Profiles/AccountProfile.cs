using Application.Dtos.Accounting;
using Application.UseCases.Account;
using AutoMapper;
using Domain.Entities.Accounting;
using Domain.Enums;

namespace Application.Profiles
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            // Entity ? DTO (with enum conversion)
            CreateMap<Account, AccountDto>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
                .ReverseMap()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => Enum.Parse<AccountType>(src.Type)));

            // Command ? Entity
            CreateMap<OpenAccountCommand, Account>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => (AccountType)src.Type));

            CreateMap<UpdateAccountCommand, Account>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => (AccountType)src.Type));
        }
    }
}
