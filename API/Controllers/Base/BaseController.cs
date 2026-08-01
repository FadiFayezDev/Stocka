using Application.Common.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Base
{
    [Authorize]
    public class BaseController : ControllerBase
    {
        public readonly IMediator _mediator;

        public BaseController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //protected Guid GetRequiredActiveBrandId()
        //{
        //    var claimValue =
        //        User.FindFirst(ClaimNames.ActiveBrandId)?.Value ??
        //        User.FindFirst("brand")?.Value ??
        //        User.FindFirst("brandId")?.Value;

        //    if (!Guid.TryParse(claimValue, out var brandId))
        //        throw new UnauthorizedAccessException("Active brand context is missing in token.");

        //    return brandId;
        //}
    }
}
