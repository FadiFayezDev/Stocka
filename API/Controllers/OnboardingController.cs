using Application.UseCases;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("Onboarding")]
    public class OnboardingController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OnboardingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("brand-owner")]
        public async Task<IActionResult> OnboardBrandOwner(
            OnboardBrandOwnerCommand command)
        {
            var brandId = await _mediator.Send(command);

            return Ok(brandId);
        }
    }
}
