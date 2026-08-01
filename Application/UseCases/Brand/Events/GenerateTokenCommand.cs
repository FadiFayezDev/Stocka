using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Auth;
using Application.Dtos.Core;
using Application.Dtos.NewSystem.Brand;
using Domain.Entities.Core;
using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCases.Brand.Events
{
    public class GenerateTokenCommand : INotification
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public Guid BrandId { get; set; }
        public Guid BranchId { get; set; }
    }

    public class GenerateTokenCommandHandler : INotificationHandler<GenerateTokenCommand>
    {
        private readonly ITokenGenerator _tokenGenerator;

        public GenerateTokenCommandHandler(ITokenGenerator tokenGenerator)
        {
            _tokenGenerator = tokenGenerator;
        }

        public async Task<CreateBrandResponseDto> Handle(GenerateTokenCommand request, CancellationToken cancellationToken)
        {
            var tokenRequest = new UserTokenDetailsDto(
                request.UserId,
                request.UserName,
                new List<string> { nameof(SystemRolesType.BrandOwner) },
                request.BrandId,
                BrandRole.Owner,
                request.BranchId);

            var token = _tokenGenerator.GenerateJWTToken(tokenRequest);

            return new CreateBrandResponseDto(token);
        }

        Task INotificationHandler<GenerateTokenCommand>.Handle(GenerateTokenCommand notification, CancellationToken cancellationToken)
        {
            return Handle(notification, cancellationToken);
        }
    }
}