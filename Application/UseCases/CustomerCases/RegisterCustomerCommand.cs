using Application.Bases;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Core;
using Application.QueryRepositories;
using AutoMapper;
using Domain.Contracts;
using Domain.Entities.Core;
using Domain.Primitives;
using MediatR;

namespace Application.UseCases.CustomerCases
{
    public class RegisterCustomerCommand : IRequest<Response<CustomerDto>>
    {
        public Guid ApplicationUserId { get; set; }
        public int LoyaltyPoints { get; set; }
    }

    public class RegisterCustomerCommandHandler : BaseHandler<ICustomerCommandRepository>, IRequestHandler<RegisterCustomerCommand, Response<CustomerDto>>
    {
        private readonly ICurrentUserContext _currentUser;
        public RegisterCustomerCommandHandler(ICustomerCommandRepository customerCommand, IMapper mapper, IUnitOfWork unitOfWork, ICurrentUserContext currentUser)
            : base(mapper, customerCommand, unitOfWork)
        {
            _currentUser = currentUser;
        }

        public async Task<Response<CustomerDto>> Handle(RegisterCustomerCommand request, CancellationToken cancellationToken)
        {
            var brandId = _currentUser.ActiveBrandId;
            var customer = new Domain.Entities.Core.Customer(request.ApplicationUserId, new BrandId(brandId), request.LoyaltyPoints);

            return await ExecuteCreateAsync<Domain.Entities.Core.Customer, CustomerDto>(
                customer,
                async (cust) => await _repo.CreateAsync(cust),
                cancellationToken);
        }
    }
}
