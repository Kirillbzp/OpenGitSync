using Microsoft.AspNetCore.Components.Authorization;
using OpenGitSync.Client.Services;
using System.Security.Claims;
using System.Text.Json;

namespace OpenGitSync.Client.Providers
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ITokenService tokenService;
        
        public CustomAuthenticationStateProvider(ITokenService tokenService) : base()
        {
            this.tokenService = tokenService;
        }
        
        public void StateChanged()
        {
            // Get the updated AuthenticationState
            Task<AuthenticationState> updatedState = GetAuthenticationStateAsync();

            // Raise the AuthenticationStateChanged event
            NotifyAuthenticationStateChanged(updatedState);
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await tokenService.GetToken();
            var identity = string.IsNullOrEmpty(token) //|| token?.Expiration < DateTime.Now
                ? new ClaimsIdentity()
                : new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            var claims = keyValuePairs.Select(kvp => new Claim(GetClaimType(kvp.Key), kvp.Value.ToString())).ToList();
            return claims;
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }

        private static string GetClaimType(string key)
        {
            return key switch
            {
                "unique_name" => ClaimTypes.Name,
                "role" => ClaimTypes.Role,
                "email" => ClaimTypes.Email,
                _ => key
            };
        }

    }
}
