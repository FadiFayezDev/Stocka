using Application.Bases;
using Application.Dtos.Core;
using Application.QueryRepositories;
using AutoMapper;
using Domain.Contracts;
using MediatR;

namespace Application.UseCases.Commands.Customer.Update
{
    public class UpdateCustomerCommand : IRequest<Response<CustomerDto>>
    {
        public Guid Id { get; set; }
        public Guid ApplicationUserId { get; set; }
        public Guid? BrandId { get; set; }
        public int LoyaltyPoints { get; set; }
    }

    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, Response<CustomerDto>>
    {
        private readonly ICustomerCommandRepository _customerCommand;
        private readonly ICustomerQueryRepository _customerQuery;
        private readonly IMapper _mapper;

        public UpdateCustomerCommandHandler(ICustomerCommandRepository customerCommand, ICustomerQueryRepository customerQuery, IMapper mapper)
        {
            _customerCommand = customerCommand;
            _customerQuery = customerQuery;
            _mapper = mapper;
        }

        public async Task<Response<CustomerDto>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            //var existingCustomer = await _customerQuery.GetByIdAsync(request.Id);
            //if (existingCustomer == null)
            //    return new Response<CustomerDto>("Customer not found");

            //existingCustomer.UserId = request.ApplicationUserId;
            //existingCustomer.BrandId = request.BrandId;
            //existingCustomer.LoyaltyPoints = request.LoyaltyPoints;

            //var updated = await _customerCommand.UpdateAsync(existingCustomer);
            //var customerDto = _mapper.Map<CustomerDto>(updated);
            //return new Response<CustomerDto>(customerDto, "Updated Successfully");

            throw new NotImplementedException();
        }
    }
}