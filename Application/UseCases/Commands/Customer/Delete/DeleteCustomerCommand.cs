using Application.Bases;
using Application.Common.Interfaces;
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

    public class DeleteCustomerCommandHandler : BaseHandler<ICustomerCommandRepository, ICustomerQueryRepository>, IRequestHandler<DeleteCustomerCommand, Response<bool>>
    {
        public DeleteCustomerCommandHandler(ICustomerCommandRepository customerCommand, ICustomerQueryRepository customerQuery, IUnitOfWork unitOfWork)
            : base(null, customerCommand, customerQuery, unitOfWork)
        {
        }

        public async Task<Response<bool>> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var existingCustomer = await _command.GetByIdAsync(request.Id);
            if (existingCustomer == null)
                return new Response<bool>(false, "Customer not found");

            return await ExecuteDeleteAsync(
                existingCustomer,
                async (cust) => await _command.DeleteAsync(cust),
                cancellationToken);
        }
    }
}
