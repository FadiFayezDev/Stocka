using Application.UseCases.Brand;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BrandsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("my")]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(await _mediator.Send(new GetUserBrandsCommand(), cancellationToken));
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> RetrieveBrandAsync(Guid id, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(await _mediator.Send(new RetrieveBrandCommand(id), cancellationToken));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBrandCommand command, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(await _mediator.Send(command, cancellationToken));
        }
    }
}
