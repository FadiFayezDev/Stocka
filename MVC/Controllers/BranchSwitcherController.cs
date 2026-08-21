using Application.Common.Interfaces;
using Application.QueryRepositories;
using Application.UseCases.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    [Authorize]
    public class BranchSwitcherController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IBranchQueryRepository _branchQuery;
        private readonly ICurrentUserContext _currentUser;

        public BranchSwitcherController(
            IMediator mediator,
            IBranchQueryRepository branchQuery,
            ICurrentUserContext currentUser)
        {
            _mediator = mediator;
            _branchQuery = branchQuery;
            _currentUser = currentUser;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Switch(Guid branchId)
        {
            var brandId = _currentUser.ActiveBrandId;

            var branch = (await _branchQuery.GetAllByBrandIdAsync(brandId))
                .FirstOrDefault(b => b.Id == branchId);

            if (branch == null)
                return Json(new { success = false, message = "الفرع المحدد غير موجود." });

            var response = await _mediator.Send(new SwitchBranchCommand { Id = branchId });

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            };

            Response.Cookies.Append("access_token", response.Token, cookieOptions);

            return Json(new { success = true, branch = branch.Name });
        }
    }
}