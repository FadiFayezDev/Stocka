using API.Attributes;
using Application.Common.Security;
using Microsoft.AspNetCore.Authorization; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Filters
{
    public class EnforceActiveBrandRouteFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // 1. استثناء الـ Controllers اللي عليها [IgnoreActiveBrandFilter]
            var hasIgnoreAttribute = context.ActionDescriptor.EndpointMetadata
                .Any(em => em is IgnoreActiveBrandFilterAttribute);

            // 2. استثناء الـ Actions اللي عليها [AllowAnonymous] (زي الـ Login)
            var isAllowAnonymous = context.ActionDescriptor.EndpointMetadata
                .Any(em => em is AllowAnonymousAttribute);

            // لو أي واحد من الاتنين موجود، كمل للـ Controller علطول
            if (hasIgnoreAttribute || isAllowAnonymous)
            {
                await next();
                return;
            }

            // 3. التحقق من المصادقة (لو مش مسجل دخول، الفلتر مش اختصاصه يمنعه، الـ [Authorize] هي اللي تمنعه)
            var user = context.HttpContext.User;
            if (user.Identity?.IsAuthenticated != true)
            {
                await next();
                return;
            }

            // 4. هل الـ Action دي أصلاً فيها بارامتر اسمه brandId؟
            // لو مفيش، يبقى الـ Filter ده ملوش لزمة هنا
            if (!TryGetRequestedBrandId(context.ActionArguments, out var requestedBrandId))
            {
                await next();
                return;
            }

            // 5. محاولة قراءة الـ Brand من الـ Token
            var claimValue =
                user.FindFirst(ClaimNames.ActiveBrandId)?.Value ??
                user.FindFirst("brand")?.Value ??
                user.FindFirst("brandId")?.Value;

            // لو التوكن مفيهوش BrandId، نمنعه (Forbid)
            if (string.IsNullOrEmpty(claimValue) || !Guid.TryParse(claimValue, out var activeBrandId))
            {
                context.Result = new ForbidResult();
                return;
            }

            // 6. المقارنة النهائية
            if (requestedBrandId != activeBrandId)
            {
                context.Result = new ForbidResult();
                return;
            }

            await next();
        }

        private static bool TryGetRequestedBrandId(IDictionary<string, object?> actionArguments, out Guid brandId)
        {
            brandId = Guid.Empty;
            if (actionArguments == null) return false;

            var argument = actionArguments
                .FirstOrDefault(a => string.Equals(a.Key, "brandId", StringComparison.OrdinalIgnoreCase));

            if (argument.Equals(default(KeyValuePair<string, object?>)) || argument.Value is null)
                return false;

            if (argument.Value is Guid guid)
            {
                brandId = guid;
                return true;
            }

            if (Guid.TryParse(argument.Value.ToString(), out var parsed))
            {
                brandId = parsed;
                return true;
            }

            return false;
        }
    }
}