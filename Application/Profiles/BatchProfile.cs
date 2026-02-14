using Application.Dtos.Products;
using Application.Features.Commands.Batch.Create;
using Application.Features.Commands.Batch.Update;
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
            CreateMap<CreateBatchCommand, Batch>();
            CreateMap<UpdateBatchCommand, Batch>();
        }
    }
}
