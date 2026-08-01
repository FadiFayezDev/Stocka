using Application.Common.Interfaces;
using Application.Dtos.Auth;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCases.Auth
{
    public class UserProfileCommand : IRequest<UserDetailsDto>
    {
    }

    public class UserProfileCommandHandler : IRequestHandler<UserProfileCommand, UserDetailsDto>
    {
        private readonly IIdentityService _identityService;
        private readonly ICurrentUserContext _userContext;

        public UserProfileCommandHandler(IIdentityService identityService, ICurrentUserContext userContext) 
        {
            this._identityService = identityService;
            this._userContext = userContext;
        }
        public async Task<UserDetailsDto> Handle(UserProfileCommand request, CancellationToken cancellationToken)
        {
            var userId = _userContext.UserId;
            var user = await _identityService.GetUserDetailsAsync(userId);
            if (user == null)
                throw new ApplicationException("user is not found");
            return user;
        }
    }
}
