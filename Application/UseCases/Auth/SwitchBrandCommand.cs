using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Auth;
using Application.QueryRepositories;
using Domain.Enums;
using MediatR;

namespace Application.UseCases.Auth
{
    public class SwitchBrandCommand : IRequest<SwitchBrandResponseDto>
    {
        public Guid BrandId { get; set; }
    }

    public class SwitchBrandCommandHandler : IRequestHandler<SwitchBrandCommand, SwitchBrandResponseDto>
    {
        private readonly ICurrentUserContext _currentUser;
        private readonly IIdentityService _identityService;
        private readonly IBranchQueryRepository _branchQueryRepository;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IBrandQueryRepository _brandQueryRepository;
        public SwitchBrandCommandHandler(
            ICurrentUserContext currentUser,
            IIdentityService identityService,
            IBranchQueryRepository branchQueryRepository,
            ITokenGenerator tokenGenerator,
            IBrandQueryRepository brandQueryRepository)
        {
            _currentUser = currentUser;
            _identityService = identityService;
            _branchQueryRepository = branchQueryRepository;
            _tokenGenerator = tokenGenerator;
            _brandQueryRepository = brandQueryRepository;
        }

        public async Task<SwitchBrandResponseDto> Handle(SwitchBrandCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;
            var canAccessAllBranches = _currentUser.CanAccessAllBranches;
                var role = _currentUser.Role;
            var brand = await _brandQueryRepository.GetByIdAsync(request.BrandId);

            if (!canAccessAllBranches)
                throw new BadRequestException("you canot ...");

            if (brand == null)
                throw new BadRequestException("brand is not found.");

            var branch = await _branchQueryRepository.GetAllByBrandIdAsync(brand.Id);
            var branchId = branch.FirstOrDefault()?.Id;
            var userDetails = await _identityService.GetUserDetailsAsync(_currentUser.UserId);

            var userToken = new UserTokenDetailsDto(
                userId, 
                userDetails.UserName, 
                userDetails.Roles, 
                request.BrandId, 
                role, 
                branchId);

            var token = _tokenGenerator.GenerateJWTToken(userToken);

            return new SwitchBrandResponseDto
            {
                Token = token
            };
        }
    }
}