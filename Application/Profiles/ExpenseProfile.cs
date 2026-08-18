using Application.Dtos.Expenses;
using Application.UseCases.ExpenseCases;
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
            CreateMap<RecordExpenseCommand, Expense>();
            CreateMap<UpdateExpenseCommand, Expense>();
        }
    }
}
