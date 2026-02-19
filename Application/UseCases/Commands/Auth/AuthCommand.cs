using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Auth;
using Application.Dtos.Core;
using Application.DTOs;
using Application.QueryRepositories;
using MediatR;

namespace Application.UseCases.Commands.Auth
{
    public class AuthCommand : IRequest<AuthResponseDTO>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class AuthCommandHandler : IRequestHandler<AuthCommand, AuthResponseDTO>
    {
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IBrandQueryRepository _brandQuery;
        private readonly IIdentityService _identityService;

        public AuthCommandHandler(IIdentityService identityService, ITokenGenerator tokenGenerator, IBrandQueryRepository brandQuery)
        {
            _identityService = identityService;
            _tokenGenerator = tokenGenerator;
            this._brandQuery = brandQuery;
        }

        public async Task<AuthResponseDTO> Handle(AuthCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.SigninUserAsync(request.UserName, request.Password);

            if (!result)
                throw new BadRequestException("Invalid username or password");

            var (userId, brandIds, firstName, lastName, userName, email, roles) = await _identityService.GetUserDetailsAsync(await _identityService.GetUserIdAsync(request.UserName));

            var brands = new List<BrandDto>();

            foreach (var brandId in brandIds)
            {
                var brand = await _brandQuery.GetByIdAsync(brandId);
                if (brand is null)
                    throw new NotFoundException($"Brand with id {brandId} not found");
                brands.Add(brand);
            }

            var userTokenDto = new UserTokenDetailsDto(
                userId,
                brandIds,
                userName,
                roles
            );

            string token = _tokenGenerator.GenerateJWTToken(userTokenDto);

            return new AuthResponseDTO()
            {
                UserId = userId,
                Brands = brands,
                Name = userName,
                Token = token
            };
        }
    }
}