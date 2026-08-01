using Application.Common.Interfaces;
using Application.Dtos.Auth;
using Application.QueryRepositories;
using Domain.Entities.Core;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Application.UseCases.Commands.Auth
{
    public class SwitchBranchCommand : IRequest<SwitchBranchResponseDto>
    {
        public Guid Id { get; set; }
    }

    public class SwitchBranchCommandHandler : IRequestHandler<SwitchBranchCommand, SwitchBranchResponseDto>
    {
        private IBranchQueryRepository _branchQueryRepository;
        private IBrandQueryRepository _brandQueryRepository;
        private ICurrentUserContext _currentUserContext;
        private IIdentityService _identityService;
        private ITokenGenerator _tokenGenerator;
        public SwitchBranchCommandHandler(
            IBranchQueryRepository branchQueryRepository,
            ICurrentUserContext currentUserContext,
            IIdentityService identityService,
            ITokenGenerator tokenGenerator,
            IBrandQueryRepository brandQueryRepository)
        {
            _branchQueryRepository = branchQueryRepository;
            _currentUserContext = currentUserContext;
            _identityService = identityService;
            _tokenGenerator = tokenGenerator;
            _brandQueryRepository = brandQueryRepository;
        }

        public async Task<SwitchBranchResponseDto> Handle(SwitchBranchCommand request, CancellationToken cancellationToken)
        {
            var brandId = _currentUserContext.ActiveBrandId;

            var user = await _identityService.GetUserDetailsAsync(_currentUserContext.UserId);
            var role = _currentUserContext.Role;

            var branchesOfBrand = await _branchQueryRepository.GetAllByBrandIdAsync(brandId);

            if (branchesOfBrand == null) 
                throw new ApplicationException("brand is not found.");

            var branch = branchesOfBrand.FirstOrDefault(b => b.Id == request.Id);

            if (branch == null)
                throw new ApplicationException("Branch is not found.");

            var userToken = new UserTokenDetailsDto(
                user.UserId,
                user.UserName,
                user.Roles,
                brandId,
                role,
                branch.Id);

            var token = _tokenGenerator.GenerateJWTToken(userToken);

            return new SwitchBranchResponseDto(token);
        }
    }
}
