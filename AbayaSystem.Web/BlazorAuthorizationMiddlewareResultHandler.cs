using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

namespace AbayaSystem.Web
{
    public class BlazorAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
    {
        public Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
        {
            // Bypass the standard ASP.NET Core challenge/redirect on initial page-load (SSR)
            // This safely hands the request over to Blazor where BoutiqueAuthStateProvider handles the UI
            return next(context);
        }
    }
}