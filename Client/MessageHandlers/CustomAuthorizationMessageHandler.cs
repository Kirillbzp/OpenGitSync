using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using OpenGitSync.Client.Providers;
using System.Net.Http.Headers;

namespace OpenGitSync.Client.MessageHandlers
{
    public class CustomAuthorizationMessageHandler : DelegatingHandler, IDisposable
    {
        private readonly IAccessTokenProvider _provider;
        private readonly NavigationManager _navigation;
        private readonly CustomAuthenticationStateProvider _authStateProvider;

        private AuthenticationStateChangedHandler _authenticationStateChangedHandler;
        private AccessToken _lastToken; 
        private AuthenticationHeaderValue _cachedHeader;
        private Uri[] _authorizedUris;
        private AccessTokenRequestOptions _tokenOptions;

        public CustomAuthorizationMessageHandler(IAccessTokenProvider provider, 
                                                 NavigationManager navigation, 
                                                 CustomAuthenticationStateProvider authStateProvider)
        {
            _provider = provider;
            _navigation = navigation;
            
            ConfigureHandler(new[] { _navigation.BaseUri });

            //Invalidate the cached _lastToken when the authentication state changes
            _authenticationStateChangedHandler = _ => { _lastToken = null; };
            authStateProvider.AuthenticationStateChanged += _authenticationStateChangedHandler;
            _authStateProvider = authStateProvider;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var now = DateTimeOffset.Now;
            if (_authorizedUris == null)
            {
                throw new InvalidOperationException($"The '{nameof(AuthorizationMessageHandler)}' is not configured. " +
                    $"Call '{nameof(AuthorizationMessageHandler.ConfigureHandler)}' and provide a list of endpoint urls to attach the token to.");
            }

            if (_authorizedUris.Any(uri => uri.IsBaseOf(request.RequestUri)))
            {
                if (_lastToken == null || now >= _lastToken.Expires.AddMinutes(-5))
                {
                    var tokenResult = _tokenOptions != null ?
                        await _provider.RequestAccessToken(_tokenOptions) :
                        await _provider.RequestAccessToken();

                    if (tokenResult.TryGetToken(out var token))
                    {
                        _lastToken = token;
                        _cachedHeader = new AuthenticationHeaderValue("Bearer", _lastToken.Value);
                    }
                    else
                    {
                        throw new AccessTokenNotAvailableException(_navigation, tokenResult, _tokenOptions?.Scopes);
                    }
                }

                // We don't try to handle 401s and retry the request with a new token automatically since that would mean we need to copy the request
                // headers and buffer the body and we expect that the user instead handles the 401s. (Also, we can't really handle all 401s as we might
                // not be able to provision a token without user interaction).
                request.Headers.Authorization = _cachedHeader;
            }

            return await base.SendAsync(request, cancellationToken);
        }

        public CustomAuthorizationMessageHandler ConfigureHandler(
                                                IEnumerable<string> authorizedUrls,
                                                IEnumerable<string> scopes = null,
                                                string returnUrl = null)
        {
            if (_authorizedUris != null)
            {
                throw new InvalidOperationException("Handler already configured.");
            }

            if (authorizedUrls == null)
            {
                throw new ArgumentNullException(nameof(authorizedUrls));
            }

            var uris = authorizedUrls.Select(uri => new Uri(uri, UriKind.Absolute)).ToArray();
            if (uris.Length == 0)
            {
                throw new ArgumentException("At least one URL must be configured.", nameof(authorizedUrls));
            }

            _authorizedUris = uris;
            var scopesList = scopes?.ToArray();
            if (scopesList != null || returnUrl != null)
            {
                _tokenOptions = new AccessTokenRequestOptions
                {
                    Scopes = scopesList,
                    ReturnUrl = returnUrl
                };
            }

            InnerHandler = new HttpClientHandler();

            return this;
        }

        void IDisposable.Dispose()
        {
            _authStateProvider.AuthenticationStateChanged -= _authenticationStateChangedHandler;
            Dispose(disposing: true);
        }
    }
}
