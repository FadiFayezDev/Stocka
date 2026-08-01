using Application.Common.Interfaces;
using Application.DTOs;
using MediatR;

namespace Application.Queries.User
{
    public class GetUserDetailsByUserNameQuery : IRequest<UserDetailsResponseDTO>
    {
        public string UserName { get; set; }
    }

    public class GetUserDetailsByUserNameQueryHandler : IRequestHandler<GetUserDetailsByUserNameQuery, UserDetailsResponseDTO>
    {
        private readonly IIdentityService _identityService;

        public GetUserDetailsByUserNameQueryHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }
        public async Task<UserDetailsResponseDTO> Handle(GetUserDetailsByUserNameQuery request, CancellationToken cancellationToken)
        {
            var (userId, personId, firstName, lastName, userName, email, roles) = await _identityService.GetUserDetailsByUserNameAsync(request.UserName);
            return new UserDetailsResponseDTO() { Id = userId, FirstName = firstName, LastName = lastName, UserName = userName, Email = email, Roles = roles };
        }
    }
}
