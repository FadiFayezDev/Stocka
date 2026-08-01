using Application.Dtos.Products;
using Application.Features.Commands.WarehouseBatch;
using Application.Features.Commands.WarehouseBatch.Create;
using Application.Features.Commands.WarehouseBatch.Update;
using AutoMapper;
using Domain.Entities.Products;

namespace Application.Profiles
{
    public class WarehouseBatchProfile : Profile
    {
        public WarehouseBatchProfile()
        {
            // Entity ? DTO
            CreateMap<WarehouseBatch, WarehouseBatchDto>().ReverseMap();

            // Command ? Entity
            CreateMap<CreateWarehouseBatchCommand, WarehouseBatch>();
            CreateMap<UpdateWarehouseBatchCommand, WarehouseBatch>();
        }
    }
}
