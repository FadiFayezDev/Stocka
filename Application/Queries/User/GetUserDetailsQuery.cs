using Application.Common.Interfaces;
using Application.DTOs;
using MediatR;

namespace Application.Queries.User
{
    public class GetUserDetailsQuery : IRequest<UserDetailsResponseDTO>
    {
        public Guid UserId { get; set; }
    }

    public class GetUserDetailsQueryHandler : IRequestHandler<GetUserDetailsQuery, UserDetailsResponseDTO>
    {
        private readonly IIdentityService _identityService;

        public GetUserDetailsQueryHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }
        public async Task<UserDetailsResponseDTO> Handle(GetUserDetailsQuery request, CancellationToken cancellationToken)
        {
            var (userId, personId, firstName, lastname, userName, email, roles) = await _identityService.GetUserDetailsAsync(request.UserId);
            return new UserDetailsResponseDTO() { Id = userId, FirstName = firstName, LastName = lastname, UserName = userName, Email = email, Roles = roles };
        }
    }
}
