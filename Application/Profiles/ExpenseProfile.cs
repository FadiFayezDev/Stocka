using Application.Dtos.Expenses;
using Application.Features.Commands.Expense;
using Application.Features.Commands.Expense.Create;
using Application.Features.Commands.Expense.Update;
using AutoMapper;
using Domain.Entities.Expenses;

namespace Application.Profiles
{
    public class ExpenseProfile : Profile
    {
        public ExpenseProfile()
        {
            // Entity ? DTO
            CreateMap<Expense, ExpenseDto>().ReverseMap();

            // Command ? Entity
            CreateMap<CreateExpenseCommand, Expense>();
            CreateMap<UpdateExpenseCommand, Expense>();
        }
    }
}
