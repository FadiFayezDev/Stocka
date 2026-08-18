using Application.Bases;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Core;
using Application.QueryRepositories;
using AutoMapper;
using Domain.Contracts;
using MediatR;

namespace Application.UseCases.CustomerCases
{
    public class UpdateCustomerProfileCommand : IRequest<Response<CustomerDto>>
    {
        public Guid Id { get; set; }
        public Guid ApplicationUserId { get; set; }
        public int LoyaltyPoints { get; set; }
    }

    public class UpdateCustomerProfileCommandHandler : BaseHandler<ICustomerCommandRepository, ICustomerQueryRepository>, IRequestHandler<UpdateCustomerProfileCommand, Response<CustomerDto>>
    {
        public UpdateCustomerProfileCommandHandler(ICustomerCommandRepository customerCommand, ICustomerQueryRepository customerQuery, IMapper mapper, IUnitOfWork unitOfWork)
            : base(mapper, customerCommand, customerQuery, unitOfWork)
        {
        }

        public async Task<Response<CustomerDto>> Handle(UpdateCustomerProfileCommand request, CancellationToken cancellationToken)
        {
            var existingCustomer = await _command.GetByIdAsync(request.Id);
            if (existingCustomer == null)
                throw new BusinessException("Customer not found");

            existingCustomer.SetLoyaltyPoints(request.LoyaltyPoints);

            return await ExecuteUpdateAsync<Domain.Entities.Core.Customer, CustomerDto>(
                existingCustomer,
                async (cust) => await _command.UpdateAsync(cust),
                cancellationToken);
        }
    }
}