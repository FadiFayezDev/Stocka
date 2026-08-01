using Application.Common.Interfaces;
using MediatR;


namespace Application.UseCases.Commands.Role.Create
{
    public class RoleCreateCommand : IRequest<bool>
    {
        public string RoleName { get; set; }
    }

    public class RoleCreateCommandHandler : IRequestHandler<RoleCreateCommand, bool>
    {
        private readonly IIdentityService _identityService;

        public RoleCreateCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }
        public async Task<bool> Handle(RoleCreateCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.CreateRoleAsync(request.RoleName);
            return result;
        }
    }
}
