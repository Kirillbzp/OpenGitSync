using Microsoft.AspNetCore.Mvc;

namespace OpenGitSync.Server.Controllers
{
    public class ApiControllerBase : ControllerBase
    {
        private readonly IApiControllerWrapper wrapper;

        public ApiControllerBase(IApiControllerWrapper wrapper)
        {
            this.wrapper = wrapper;
        }

        protected long UserId => wrapper.UserId(User);
    }
}
