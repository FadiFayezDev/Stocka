using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Core;
using Application.QueryRepositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCases.Brand
{
    public class RetrieveBrandCommand : IRequest<BrandDto>
    {
        public Guid BrandId { get; set; }

        public RetrieveBrandCommand(Guid brandId)
        {
            BrandId = brandId;
        }
    }

    public class RetrieveBrandCommandHandler : IRequestHandler<RetrieveBrandCommand, BrandDto>
    {
        private readonly IBrandQueryRepository _brandQuery;

        public RetrieveBrandCommandHandler(IBrandQueryRepository brandQuery)
        {
            _brandQuery = brandQuery;
        }

        public async Task<BrandDto> Handle(RetrieveBrandCommand request, CancellationToken cancellationToken)
        {
            var brand = await _brandQuery.GetByIdAsync(request.BrandId);
            if (brand == null)
                throw new BusinessException("brand is not found.");
            return brand;
        }
    }
}