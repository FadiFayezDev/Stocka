using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos;
using Application.Dtos.Auth;
using Application.Dtos.Core;
using Application.Dtos.NewSystem.Brand;
using Application.UseCases.Auth;
using AutoMapper;
using Domain.Entities.Core;
using Domain.Entities.Products;
using Domain.Enums;
using Domain.Repositories.Commands;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCases.Brand
{
    public class CreateBrandCommand : IRequest<CreateBrandResponseDto>
    {
        public string Name { get; set; } = null!;
        public string? Slug { get; set; }
    }

    public class CreateBrandCommandHanlder : IRequestHandler<CreateBrandCommand, CreateBrandResponseDto>
    {
        private readonly IIdentityService _identityService;
        private readonly ICurrentUserContext _currentUserContext;
        private readonly IBrandCommandRepository _brandRepository;
        private readonly IBranchCommandRepository _branchRepository;
        private readonly IWarehouseCommandRepository _warehouseRepository;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        private readonly ILogger<OnboardBrandOwnerCommandHandler> _logger;
        private readonly IMediator _mediator;
        public CreateBrandCommandHanlder(
            IIdentityService identityService, 
            ICurrentUserContext currentUserContext,
            IBrandCommandRepository brandRepository, 
            IBranchCommandRepository branchRepository, 
            IWarehouseCommandRepository warehouseRepository, 
            ITokenGenerator tokenGenerator, 
            IMapper mapper, 
            IUnitOfWork unitOfWork, 
            ILogger<OnboardBrandOwnerCommandHandler> logger)
        {
            _identityService = identityService;
            _currentUserContext = currentUserContext;
            _brandRepository = brandRepository;
            _branchRepository = branchRepository;
            _warehouseRepository = warehouseRepository;
            _tokenGenerator = tokenGenerator;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<CreateBrandResponseDto> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
        {
            var user = await _identityService.GetUserDetailsAsync(_currentUserContext.UserId);

            if (user == null)
                throw new BusinessException("User is not found.");

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var brand = new Domain.Entities.Core.Brand(request.Name, request.Slug); 
                var branch = new Branch(brand.Id, "Main Branch");

                brand.AddBranch(branch);
                brand.AddMember(user.UserId, BrandRole.Owner);
                await _brandRepository.CreateAsync(brand);

                var warehouse = new Warehouse(brand.Id, "Main Warehouse", WarehouseType.Shop, "unnon");

                await _warehouseRepository.CreateAsync(warehouse);

                var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync();

                var tokenRequest = new UserTokenDetailsDto(
                    user.UserId,
                    user.UserName,
                    new List<string> { nameof(SystemRolesType.BrandOwner) },
                    brand.Id.Value,
                    BrandRole.Owner,
                    branch.Id.Value);

                var token = _tokenGenerator.GenerateJWTToken(tokenRequest);

                return new CreateBrandResponseDto(token);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Onboarding failed");
                throw;
            }

        }
    }
}