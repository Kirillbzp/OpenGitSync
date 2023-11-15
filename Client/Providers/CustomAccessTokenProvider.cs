using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using OpenGitSync.Client.Services;
using System.Threading.Tasks;

namespace OpenGitSync.Client.Providers
{
    public class CustomAccessTokenProvider : IAccessTokenProvider
    {
        private readonly ITokenService _tokenService;

        public CustomAccessTokenProvider(ITokenService tokenService) 
        {
            _tokenService = tokenService;
        }
        
        public async ValueTask<AccessTokenResult> RequestAccessToken()
        {
            var token = await _tokenService.GetToken();
            var accessToken = new AccessToken()
            {
                Value = token,
                Expires = new DateTimeOffset(DateTime.Now.AddDays(1))
            };
            AccessTokenResult accessTokenResult;

            if (!string.IsNullOrEmpty(token))
            {
                accessTokenResult = new AccessTokenResult(AccessTokenResultStatus.Success, accessToken, "/");
            }
            else
            {
                accessTokenResult = new AccessTokenResult(AccessTokenResultStatus.RequiresRedirect, accessToken, "/login");
            }
            return accessTokenResult;
        }

        public async ValueTask<AccessTokenResult> RequestAccessToken(AccessTokenRequestOptions options)
        {
            return await RequestAccessToken();
        }
    }
}
