using Application.Dtos.Expenses;
using Application.UseCases.ExpenseCategoryCases;
using AutoMapper;
using Domain.Entities.Expenses;

namespace Application.Profiles
{
    public class ExpenseCategoryProfile : Profile
    {
        public ExpenseCategoryProfile()
        {
            // Entity ? DTO
            CreateMap<ExpenseCategory, ExpenseCategoryDto>().ReverseMap();

            // Command ? Entity
            CreateMap<RegisterExpenseCategoryCommand, ExpenseCategory>();
            CreateMap<UpdateExpenseCategoryCommand, ExpenseCategory>();
        }
    }
}
