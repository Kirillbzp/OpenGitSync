using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using System.Net;
using System.Runtime.Serialization;
using Toolbelt.Blazor;

public class HttpInterceptorService
{
    private readonly HttpClientInterceptor _interceptor;
    private readonly NavigationManager _navManager;
    private readonly ToastService _toastService;

    public HttpInterceptorService(HttpClientInterceptor interceptor, 
                                  NavigationManager navManager, 
                                  ToastService toastService)
    {
        _interceptor = interceptor;
        _navManager = navManager;
        _toastService = toastService;
    }

    public void RegisterEvent() => _interceptor.AfterSend += InterceptResponse;

    private void InterceptResponse(object sender, HttpClientInterceptorEventArgs e)
    {
        string message = string.Empty;
        if (!e.Response.IsSuccessStatusCode)
        {
            var statusCode = e.Response.StatusCode;

            switch (statusCode)
            {
                case HttpStatusCode.Unauthorized:
                    _navManager.NavigateTo("/logout");
                    message = "User is not authorized";
                    break;

                default:
                    message = e.Response.ReasonPhrase;
                    //_navManager.NavigateTo($"/error?ErrorMessage={Uri.EscapeDataString(message)}");
                    break;
            }

            _toastService.Notify(new(ToastType.Warning, message));
            throw new HttpResponseException(message);
        }
    }

    public void DisposeEvent() => _interceptor.AfterSend -= InterceptResponse;
}

[Serializable]
internal class HttpResponseException : Exception
{
    public HttpResponseException()
    {
    }
    public HttpResponseException(string message)
        : base(message)
    {
    }
    public HttpResponseException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
    protected HttpResponseException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}