using Microsoft.Extensions.Primitives;
using System.Security.Claims;

namespace OpenGitSync.Server.Helpers
{
    public interface IRequestHelper
    {
        string GetRefreshToken(HttpRequest request);
        string GetBearerToken(HttpRequest request);
        string GetHeaderOrEmpty(HttpRequest request, string header);
        string GetIpAddress(HttpContext context);
        string GetMacAddress(HttpContext context);

        T GetClaimValue<T>(ClaimsPrincipal user, string claimId, T defaultValue, RequestHelper.Parser<T> parser);
        T GetHeaderValue<T>(HttpRequest request, string header, T defaultValue, RequestHelper.Parser<T> parser);
    }

    public class RequestHelper : IRequestHelper
    {
        private const string AuthHeader = "Authorization";
        private const string RefreshHeader = "refreshToken";
        private const string MacAddressHeader = "MacAddress";

        public delegate bool Parser<T>(string input, out T b);

        public T GetClaimValue<T>(ClaimsPrincipal user, string claimId, T defaultValue, Parser<T> parser)
        {
            var claim = user.FindFirst(claimId);
            if (claim != null && parser(claim.Value, out T value))
                return value;
            return defaultValue;
        }

        public T GetHeaderValue<T>(HttpRequest request, string header, T defaultValue, Parser<T> parser)
        {
            var ret = request.Headers[header];
            if (ret != StringValues.Empty && parser(ret, out T value))
            {
                return value;
            }
            return defaultValue;
        }

        public string GetHeaderOrEmpty(HttpRequest request, string header)
        {
            if (request.Headers.TryGetValue(header, out var headerValue))
            {
                return headerValue.FirstOrDefault();
            }
            return string.Empty;
        }

        public string GetBearerToken(HttpRequest request)
        {
            return GetHeaderOrEmpty(request, AuthHeader);
        }

        public string GetRefreshToken(HttpRequest request)
        {
            return GetHeaderOrEmpty(request, RefreshHeader);
        }

        public string GetIpAddress(HttpContext context)
        {
            var ipAddress = GetHeaderOrEmpty(context.Request, "X-Forwarded-For");
            return !string.IsNullOrWhiteSpace(ipAddress) ? ipAddress : context.Connection.RemoteIpAddress.ToString();
        }

        public string GetMacAddress(HttpContext context)
        {
            return GetHeaderOrEmpty(context.Request, MacAddressHeader);
        }

    }
}
