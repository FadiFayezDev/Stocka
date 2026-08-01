using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.QueryRepositories;
using Domain.Primitives;
using Domain.Repositories.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Category
{
    public class AssignCategoryCommand : IRequest<bool>
    {
        public Guid ProductId { get; set; }
        public Guid CategoryId { get; set; }
    }

    public class AssignCategoryCommandHandler : IRequestHandler<AssignCategoryCommand, bool>
    {
        private readonly IProductCommandRepository _productCommand;
        private readonly IProductCategoryQueryRepository _productCategoryQuery;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AssignCategoryCommandHandler> _logger;

        public AssignCategoryCommandHandler(
            IProductCommandRepository productCommand,
            IUnitOfWork unitOfWork,
            ILogger<AssignCategoryCommandHandler> logger,
            IProductCategoryQueryRepository productCategoryQuery)
        {
            _productCommand = productCommand; 
            _productCategoryQuery = productCategoryQuery;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<bool> Handle(AssignCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var product = await _productCommand.GetByIdAsync(request.ProductId);
                if (product == null)
                    throw new BusinessException("Product not found.");

                var category = await _productCategoryQuery.GetByIdAsync(request.CategoryId);
                if (category == null)
                    throw new BusinessException("Category not found.");

                product.ChangeCategory(new ProductCategoryId(request.CategoryId));

                await _productCommand.UpdateAsync(product);
                var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Category assigned to product successfully");

                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to assign category to product");
                throw;
            }
        }
    }
}
