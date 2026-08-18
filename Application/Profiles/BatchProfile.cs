using Application.Dtos.Products;
using Application.UseCases.BatchCases;
using AutoMapper;
using Domain.Entities.Products;

namespace Application.Profiles
{
    public class BatchProfile : Profile
    {
        public BatchProfile()
        {
            // Entity ? DTO
            CreateMap<Batch, BatchDto>().ReverseMap();

            // Command ? Entity
            CreateMap<RegisterBatchCommand, Batch>();
            CreateMap<UpdateBatchCommand, Batch>();
        }
    }
}
