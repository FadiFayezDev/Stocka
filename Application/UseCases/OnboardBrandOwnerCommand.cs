using Application.Bases;
using Application.Common.Interfaces;
using Application.Dtos;
using Application.Dtos.Auth;
using Application.Dtos.Core;
using AutoMapper;
using Domain.Entities.Core;
using Domain.Entities.Products;
using Domain.Enums;
using Domain.Repositories.Commands;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;
using System.Text;

namespace Application.UseCases
{
    public class OnboardBrandOwnerCommand : IRequest<OnboardBrandOwnerDto>
    {
        #region User Details
        [Required]
        public string FirstName { get; set; } = null!; 
        [Required]
        public string LastName { get; set; } = null!;
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public string Username { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
        #endregion

        #region Brand Details
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string Slug { get; set; } = null!;
        #endregion
    }

    public class OnboardBrandOwnerCommandHandler : IRequestHandler<OnboardBrandOwnerCommand, OnboardBrandOwnerDto>
    {
        private readonly IIdentityService _identityService;
        private readonly IBrandCommandRepository _brandRepository;
        private readonly IBranchCommandRepository _branchRepository;
        private readonly IWarehouseCommandRepository _warehouseRepository;

        private readonly ITokenGenerator _tokenGenerator;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        private readonly ILogger<OnboardBrandOwnerCommandHandler> _logger;

        public OnboardBrandOwnerCommandHandler(IIdentityService identityService, IBrandCommandRepository brandRepository, IBranchCommandRepository branchRepository, IWarehouseCommandRepository warehouseRepository, ITokenGenerator tokenGenerator, IMapper mapper, IUnitOfWork unitOfWork, ILogger<OnboardBrandOwnerCommandHandler> logger)
        {
            _identityService = identityService;
            _brandRepository = brandRepository;
            _branchRepository = branchRepository;
            _warehouseRepository = warehouseRepository;
            _tokenGenerator = tokenGenerator;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<OnboardBrandOwnerDto> Handle(OnboardBrandOwnerCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                //if(!await _identityService.RoleIsExistAsync(nameof(SystemRolesType.BrandOwner)))
                //    await _identityService.CreateRoleAsync(nameof(SystemRolesType.BrandOwner));

                var user = new CreateUserDto
                (
                    request.FirstName,
                    request.LastName, 
                    request.Username,
                    request.Password,
                    request.Email,
                    new List<string> { nameof(SystemRolesType.BrandOwner) }
                );

                var createdUser = await _identityService.CreateUserAsync(user);

                var brand = new Brand(request.Name, request.Slug);

                var branch = new Branch(brand.Id, "Main Branch");

                var warehouse = new Warehouse(branch.Id, brand.Id, "Main Warehouse", WarehouseType.Shop);

                branch.AddWarehouse(warehouse);
                brand.AddMember(createdUser.UserId, nameof(SystemRolesType.BrandOwner));
                brand.AddBranch(branch);

                await _brandRepository.CreateAsync(brand);

                var tokenRequest = new UserTokenDetailsDto
                (
                    createdUser.UserId,
                    new List<Guid> { brand.Id },
                    request.Username,
                    new List<string> { nameof(SystemRolesType.BrandOwner) }
                );

                var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync();

                var token = _tokenGenerator.GenerateJWTToken(tokenRequest);

                return new OnboardBrandOwnerDto
                (
                    token,
                    new BrandDto { Id = brand.Id, Name = brand.Name, Slug = brand.Slug, CreatedAt = brand.CreatedAt }
                );
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