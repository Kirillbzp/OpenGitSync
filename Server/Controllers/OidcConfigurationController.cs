using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OpenGitSync.Server.Controllers
{
    [AllowAnonymous]
    [Route("_configuration")]
    public class OidcConfigurationController : ApiControllerBase
    {
        public OidcConfigurationController(IApiControllerWrapper wrapper, IClientRequestParametersProvider clientRequestParametersProvider) : base(wrapper)
        {
            ClientRequestParametersProvider = clientRequestParametersProvider;
        }

        public IClientRequestParametersProvider ClientRequestParametersProvider { get; }

        [HttpGet("{clientId}")]
        public IActionResult GetClientRequestParameters([FromRoute] string clientId)
        {
            var parameters = ClientRequestParametersProvider.GetClientParameters(HttpContext, clientId);
            return Ok(parameters);
        }
    }
}