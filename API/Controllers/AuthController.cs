using API.Attributes;
using API.Controllers.Base;
using Application.Dtos.Auth;
using Application.DTOs;
using Application.UseCases.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        public AuthController(IMediator mediator) : base(mediator)
        {
        }

        [IgnoreActiveBrandFilter]
        [AllowAnonymous]
        [HttpPost("Login")]
        [ProducesDefaultResponseType(typeof(AuthResponseDTO))]
        public async Task<IActionResult> Login([FromBody] AuthCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [Authorize]
        [HttpPost("switch-brand")]
        [ProducesDefaultResponseType(typeof(SwitchBrandResponseDto))]
        public async Task<IActionResult> SwitchBrand([FromBody] SwitchBrandCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [Authorize]
        [HttpPost("switch-branch")]
        [ProducesDefaultResponseType(typeof(SwitchBrandResponseDto))]
        public async Task<IActionResult> Switchbranch([FromBody] SwitchBranchCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [Authorize]
        [HttpGet("me")]
        [ProducesDefaultResponseType(typeof(UserDetailsDto))]
        public async Task<IActionResult> Me()
        {
            return Ok(await _mediator.Send(new UserProfileCommand()));
        }

        //[HttpPost("onboard-brand")]
        //public Task<IActionResult> OnboardBrand
    }
}
