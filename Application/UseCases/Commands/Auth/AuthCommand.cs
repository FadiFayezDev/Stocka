using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Auth;
using Application.Dtos.Core;
using Application.DTOs;
using Application.QueryRepositories;
using Domain.Enums;
using MediatR;

namespace Application.UseCases.Commands.Auth
{
    public class AuthCommand : IRequest<AuthResponseDTO>
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class AuthCommandHandler : IRequestHandler<AuthCommand, AuthResponseDTO>
    {
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IBrandQueryRepository _brandQuery;
        private readonly IIdentityService _identityService;

        public AuthCommandHandler(
            IIdentityService identityService,
            ITokenGenerator tokenGenerator,
            IBrandQueryRepository brandQuery)
        {
            _identityService = identityService;
            _tokenGenerator = tokenGenerator;
            _brandQuery = brandQuery;
        }

        public async Task<AuthResponseDTO> Handle(AuthCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.SigninUserAsync(request.UserName, request.Password);

            if (!result)
                throw new BadRequestException("Invalid username or password");

            var userId = await _identityService.GetUserIdAsync(request.UserName);
            var userDetails = await _identityService.GetUserDetailsAsync(userId);
            var memberships = await _identityService.GetBrandMembershipsAsync(userId);

            if (memberships.Count == 0)
                throw new BadRequestException("User has no brand membership.");

            var defaultMembership = memberships
                .OrderBy(m => GetBrandRolePriority(m.Role))
                .First();

            Guid? activeBranchId = null;
            if (defaultMembership.Role is BrandRole.Manager or BrandRole.Cashier or BrandRole.Viewer)
            {
                activeBranchId = await _identityService.GetEmployeeBranchIdAsync(userId, defaultMembership.BrandId);
                if (!activeBranchId.HasValue)
                    throw new BadRequestException("No branch assignment found for the current user in this brand.");
            }

            var brands = new List<BrandShortDto>();
            foreach (var brandId in userDetails.BrandIds)
            {
                var brand = await _brandQuery.GetByIdAsync(brandId);
                if (brand is null)
                    throw new NotFoundException($"Brand with id {brandId} not found");

                brands.Add(new BrandShortDto { Id = brand.Id, Name = brand.Name});
            }

            var userTokenDto = new UserTokenDetailsDto(
                userId, 
                userDetails.UserName,
                userDetails.Roles, 
                defaultMembership.BrandId,
                defaultMembership.Role,
                activeBranchId
             );

            var token = _tokenGenerator.GenerateJWTToken(userTokenDto);

            return new AuthResponseDTO
            {
                Name = userDetails.UserName,
                Token = token,
                Brands = brands,
            };
        }

        private static int GetBrandRolePriority(BrandRole role)
        {
            return role switch
            {
                BrandRole.Owner => 0,
                BrandRole.BrandAdmin => 1,
                BrandRole.Manager => 2,
                BrandRole.Cashier => 3,
                BrandRole.Viewer => 4,
                _ => 99
            };
        }
    }
}
