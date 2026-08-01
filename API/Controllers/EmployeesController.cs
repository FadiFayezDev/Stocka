using API.Controllers.Base;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EmployeesController : BaseController
    {
        public EmployeesController(IMediator mediator) : base(mediator)
        {
        }

        //[HttpGet]
        //public Task<IActionResult> Get()
        //{

        //}
    }
}
