using BuildingBlocks.Application.Common.Interfaces;
using BuildingBlocks.Application.Common.Models;
using Identity.Application.Repositories.Queries;
using Identity.Contracts.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Application.UseCases.Customers
{
    internal record ListCustomersQuery : IRequest<PaginatedResult<CustomerDto>>
    {
    }

    internal class ListCustomersQueryHandler : IRequestHandler<ListCustomersQuery, PaginatedResult<CustomerDto>>
    {
        private readonly ICustomerQueryRepository _customerQuery;
        private readonly ICurrentUserContext _currentUserContext;

        public ListCustomersQueryHandler(ICustomerQueryRepository customerQuery, ICurrentUserContext currentUserContext)
        {
            _customerQuery = customerQuery;
            _currentUserContext = currentUserContext;
        }
        public async Task<PaginatedResult<CustomerDto>> Handle(ListCustomersQuery request, CancellationToken cancellationToken)
        {
            var organizationId = _currentUserContext.ActiveOrganizationId;
            return await _customerQuery.GetAllPagedAsync(organizationId, 1, 100);
        }
    }
}
