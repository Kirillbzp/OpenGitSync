using OpenGitSync.Server.Helpers;
using System.Security.Claims;

namespace OpenGitSync.Server.Controllers
{
    public interface IApiControllerWrapper
    {
        string UserId(ClaimsPrincipal user);
    }

    public class ApiControllerWrapper : IApiControllerWrapper
    {
        public const string NoUserId = "";

        private readonly IRequestHelper requestHelper;

        public ApiControllerWrapper(IRequestHelper requestHelper)
        {
            this.requestHelper = requestHelper;
        }

        public string UserId(ClaimsPrincipal user)
        {
            return requestHelper.GetStringClaimValue(user, ClaimTypes.Sid, NoUserId);
        }
    }
}
