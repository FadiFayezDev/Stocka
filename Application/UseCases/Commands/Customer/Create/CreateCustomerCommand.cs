using Application.Bases;
using Application.Dtos.Core;
using Application.QueryRepositories;
using AutoMapper;
using Domain.Contracts;
using MediatR;

namespace Application.UseCases.Commands.Customer.Create
{
    public class CreateCustomerCommand : IRequest<Response<CustomerDto>>
    {
        public Guid ApplicationUserId { get; set; }
        public Guid? BrandId { get; set; }
        public int LoyaltyPoints { get; set; }
    }

    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Response<CustomerDto>>
    {
        private readonly ICustomerCommandRepository _customerCommand;
        private readonly ICustomerQueryRepository _customerQuery;
        private readonly IMapper _mapper;

        public CreateCustomerCommandHandler(ICustomerCommandRepository customerCommand, ICustomerQueryRepository customerQuery, IMapper mapper)
        {
            _customerCommand = customerCommand;
            _customerQuery = customerQuery;
            _mapper = mapper;
        }

        public async Task<Response<CustomerDto>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = new Domain.Entities.Core.Customer
            {
                UserId = request.ApplicationUserId,
                BrandId = request.BrandId,
                LoyaltyPoints = request.LoyaltyPoints
            };

            var created = await _customerCommand.CreateAsync(customer);
            var customerDto = _mapper.Map<CustomerDto>(created);
            return new Response<CustomerDto>(customerDto, "Created Successfully");
        }
    }
}
