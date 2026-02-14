using Application.Dtos.Expenses;
using Application.Features.Commands.ExpenseCategory;
using Application.Features.Commands.ExpenseCategory.Create;
using Application.Features.Commands.ExpenseCategory.Update;
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
            CreateMap<CreateExpenseCategoryCommand, ExpenseCategory>();
            CreateMap<UpdateExpenseCategoryCommand, ExpenseCategory>();
        }
    }
}
