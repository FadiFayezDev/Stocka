using Application.Bases;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Products;
using Application.QueryRepositories;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCases.Commands.ProductCategory.PartialUpdate
{
    public class PartialUpdateProductCategoryCommand : IRequest<Response<ProductCategoryDto>>
    {
        public Guid Id { get; set; }
        public string? Name { get; set; } = null!;
        public bool? IsActive { get; set; }
    }

    public class PartialUpdateProductCategoryCommandHandler : BaseHandler<IProductCategoryCommandRepository, IProductCategoryQueryRepository>, IRequestHandler<PartialUpdateProductCategoryCommand, Response<ProductCategoryDto>>
    {
        public PartialUpdateProductCategoryCommandHandler(IMapper mapper, IProductCategoryCommandRepository command, IProductCategoryQueryRepository query, IUnitOfWork work) : base(mapper, command, query, work)
        {
        }

        public async Task<Response<ProductCategoryDto>> Handle(PartialUpdateProductCategoryCommand request, CancellationToken cancellationToken)
        {
            var existing = await _command.GetByIdAsync(request.Id);
            if (existing == null)
                throw new BusinessException("Product category not found.");

            if (request.Name != null)
                existing.UpdateName(request.Name);

            if (request.IsActive.HasValue)
                existing.UpdateActive(request.IsActive.Value);

            return await ExecuteUpdateAsync<Domain.Entities.Products.ProductCategory, ProductCategoryDto>(
                existing,
                async (pc) => await _command.UpdateAsync(pc),
                cancellationToken);
        }
    }
}