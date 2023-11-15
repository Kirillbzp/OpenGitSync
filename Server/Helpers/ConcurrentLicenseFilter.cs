using Microsoft.AspNetCore.Mvc.Filters;

namespace OpenGitSync.Server.Helpers
{
    public class ConcurrentLicenseFilter : IAsyncAuthorizationFilter
    {
        public ConcurrentLicenseFilter() 
        {
            
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var requestHelper = context.HttpContext.RequestServices.GetService<IRequestHelper>();
            var bearerToken = requestHelper.GetBearerToken(context.HttpContext.Request);

        }
    }
}
