using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.UserCases
{
    public class RemoveUserCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }

    public class RemoveUserCommandHandler : IRequestHandler<RemoveUserCommand, bool>
    {
        private readonly IIdentityService _identityService;

        public RemoveUserCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }
        public async Task<bool> Handle(RemoveUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.DeleteUserAsync(request.Id);

            return result;
        }
    }
}
