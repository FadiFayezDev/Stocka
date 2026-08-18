using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Products;
using AutoMapper;
using Domain.Entities.Products;
using Domain.Primitives;
using Domain.Repositories.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Category
{
    public class RegisterProductCategoryCommand : IRequest<ProductCategoryDto>
    {
        public string Name { get; set; } = null!;
    }

    public class RegisterProductCategoryCommandHanlder : IRequestHandler<RegisterProductCategoryCommand, ProductCategoryDto>
    {
        private readonly ICurrentUserContext _currentUserContext;
        private readonly IProductCategoryCommandRepository _categoryCommand;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProductCategoryDto> _logger;

        public RegisterProductCategoryCommandHanlder(
            ICurrentUserContext currentUserContext,
            IProductCategoryCommandRepository categoryCommand,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            ILogger<ProductCategoryDto> logger)
        {
            _currentUserContext = currentUserContext;
            _categoryCommand = categoryCommand;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ProductCategoryDto> Handle(RegisterProductCategoryCommand request, CancellationToken cancellationToken)
        {
            var brandId = _currentUserContext.ActiveBrandId;
            try
            {
                var category = new ProductCategory(new BrandId(brandId), request.Name);

                await _categoryCommand.CreateAsync(category);

                var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Category is created");

                var dto = _mapper.Map<ProductCategoryDto>(category);

                return dto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Category creation failed");
                throw;
            }
        }
    }
}
