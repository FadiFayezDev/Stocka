using Application.Common.Interfaces;
using Application.Dtos.Core;
using Application.QueryRepositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCases.Brand
{
    public class GetUserBrandsCommand : IRequest<List<BrandDto>>
    {
    }

    public class GetUserBrandsCommandHandler : IRequestHandler<GetUserBrandsCommand, List<BrandDto>>
    {
        private readonly IBrandQueryRepository _brandQuery;
        private readonly ICurrentUserContext _userContext;

        public GetUserBrandsCommandHandler(IBrandQueryRepository brandQuery, ICurrentUserContext userContext)
        {
            _brandQuery = brandQuery;
            _userContext = userContext;
        }

        public async Task<List<BrandDto>> Handle(GetUserBrandsCommand request, CancellationToken cancellationToken)
        {
            var UserId = _userContext.UserId;
            var brands = await _brandQuery.GetAllUserBrandsAsync(UserId);
            return brands.ToList();
        }
    }
}
