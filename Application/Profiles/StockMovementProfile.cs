using Application.Dtos.Products;
using Application.Features.Commands.StockMovement;
using Application.Features.Commands.StockMovement.Create;
using Application.Features.Commands.StockMovement.Update;
using AutoMapper;
using Domain.Entities.Products;
using Domain.Enums;

namespace Application.Profiles
{
    public class StockMovementProfile : Profile
    {
        public StockMovementProfile()
        {
            // Entity ? DTO (with enum conversion)
            CreateMap<StockMovement, StockMovementDto>()
                .ForMember(dest => dest.MovementType, opt => opt.MapFrom(src => src.MovementType.ToString()))
                .ReverseMap()
                .ForMember(dest => dest.MovementType, opt => opt.MapFrom(src => Enum.Parse<StockMovementType>(src.MovementType)));

            // Command ? Entity
            CreateMap<CreateStockMovementCommand, StockMovement>()
                .ForMember(dest => dest.MovementType, opt => opt.MapFrom(src => Enum.Parse<StockMovementType>(src.MovementType)));

            CreateMap<UpdateStockMovementCommand, StockMovement>()
                .ForMember(dest => dest.MovementType, opt => opt.MapFrom(src => Enum.Parse<StockMovementType>(src.MovementType)));
        }
    }
}
