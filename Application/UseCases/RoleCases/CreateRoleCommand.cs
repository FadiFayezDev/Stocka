using Application.Common.Interfaces;
using MediatR;


namespace Application.UseCases.RoleCases
{
    public class CreateRoleCommand : IRequest<bool>
    {
        public string RoleName { get; set; }
    }

    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, bool>
    {
        private readonly IIdentityService _identityService;

        public CreateRoleCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }
        public async Task<bool> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.CreateRoleAsync(request.RoleName);
            return result;
        }
    }
}
