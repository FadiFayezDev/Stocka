using Application.Bases;
using Application.Dtos.Core;
using Application.QueryRepositories;
using Domain.Contracts;
using MediatR;

namespace Application.UseCases.Commands.Customer.Delete
{
    public class DeleteCustomerCommand : IRequest<Response<bool>>
    {
        public Guid Id { get; set; }
    }

    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, Response<bool>>
    {
        private readonly ICustomerCommandRepository _customerCommand;
        private readonly ICustomerQueryRepository _customerQuery;

        public DeleteCustomerCommandHandler(ICustomerCommandRepository customerCommand, ICustomerQueryRepository customerQuery)
        {
            _customerCommand = customerCommand;
            _customerQuery = customerQuery;
        }

        public async Task<Response<bool>> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var existingCustomer = await _customerQuery.GetByIdAsync(request.Id);
            if (existingCustomer == null)
                return new Response<bool>("Customer not found");

            //await _customerCommand.DeleteAsync(existingCustomer);
            return new Response<bool>(true, "Deleted Successfully");
        }
    }
}
