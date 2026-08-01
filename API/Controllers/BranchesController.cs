using API.Controllers.Base;
using Application.UseCases.BranchCases;
using Application.UseCases.ProductCases;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class BranchesController : BaseController
    {
        public BranchesController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public async Task<IActionResult> ListBranches()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(await _mediator.Send(new ListBranchesCommand()));
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> RetrieveBranch(Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(await _mediator.Send(new RetrieveBranchCommand(id)));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductCommand command, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(await _mediator.Send(command, cancellationToken));
        }

        [HttpPatch]
        public async Task<IActionResult> PartialUpdate([FromBody] PartialUpdateProductCommand command, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(await _mediator.Send(command, cancellationToken));
        }
    }
}