using DB;
using OpenGitSync.Server.Helpers;
using System.Security.Claims;

namespace OpenGitSync.Server.Controllers
{
    public interface IApiControllerWrapper
    {
        long UserId(ClaimsPrincipal user);
        
    }
    public class ApiControllerWrapper : IApiControllerWrapper
    {
        public const long NoUserId = 0;
        
        private readonly IRequestHelper requestHelper;
        

        public ApiControllerWrapper(IRequestHelper requestHelper)
        {
            this.requestHelper = requestHelper;
        }

        public long UserId(ClaimsPrincipal user)
        {
            return requestHelper.GetClaimValue(user, ClaimTypes.Sid, NoUserId, long.TryParse);
        }

    }
}
