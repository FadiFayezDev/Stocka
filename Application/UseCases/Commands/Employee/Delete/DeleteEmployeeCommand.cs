using Application.Bases;
using Application.Dtos.Core;
using Domain.Contracts;
using MediatR;

namespace Application.UseCases.Commands.Employee.Delete
{
    public class DeleteEmployeeCommand : IRequest<Response<bool>>
    {
        public Guid Id { get; set; }
    }

    public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, Response<bool>>
    {
        private readonly IEmployeeCommandRepository _repository;

        public DeleteEmployeeCommandHandler(IEmployeeCommandRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response<bool>> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            //var existingEmployee = await _repository.GetByIdAsync(request.Id);
            //if (existingEmployee == null)
            //    return new Response<bool>("Employee not found");

            //await _repository.DeleteAsync(existingEmployee);
            //return new Response<bool>(true, "Deleted Successfully");
            throw new NotImplementedException();
        }
    }
}
