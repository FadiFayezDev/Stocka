using Application.Bases;
using Application.Common.Interfaces;
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

    public class CreateCustomerCommandHandler : BaseHandler<ICustomerCommandRepository, ICustomerQueryRepository>, IRequestHandler<CreateCustomerCommand, Response<CustomerDto>>
    {
        public CreateCustomerCommandHandler(ICustomerCommandRepository customerCommand, ICustomerQueryRepository customerQuery, IMapper mapper, IUnitOfWork unitOfWork)
            : base(mapper, customerCommand, customerQuery, unitOfWork)
        {
        }

        public async Task<Response<CustomerDto>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = new Domain.Entities.Core.Customer
            {
                UserId = request.ApplicationUserId,
                BrandId = request.BrandId,
                LoyaltyPoints = request.LoyaltyPoints
            };

            return await ExecuteCreateAsync<Domain.Entities.Core.Customer, CustomerDto>(
                customer,
                async (cust) => await _command.CreateAsync(cust),
                cancellationToken);
        }
    }
}
