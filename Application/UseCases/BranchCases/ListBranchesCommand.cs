using Application.Common.Interfaces;
using Application.Dtos.Core;
using Application.QueryRepositories;
using Application.UseCases.Brand;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCases.BranchCases
{
    public class ListBranchesCommand : IRequest<List<BranchDto>>
    {
    }

    public class ListBranchesCommandHandler : IRequestHandler<ListBranchesCommand, List<BranchDto>>
    {
        private readonly IBranchQueryRepository _branchQuery;
        private readonly ICurrentUserContext _userContext;

        public ListBranchesCommandHandler(IBranchQueryRepository branchQuery, ICurrentUserContext userContext)
        {
            _branchQuery = branchQuery;
            _userContext = userContext;
        }

        public async Task<List<BranchDto>> Handle(ListBranchesCommand request, CancellationToken cancellationToken)
        {
            var brandId = _userContext.ActiveBrandId;
            var branch = await _branchQuery.GetAllByBrandIdAsync(brandId);
            return branch.ToList();
        }
    }
}