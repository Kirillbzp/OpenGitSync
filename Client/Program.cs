using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using OpenGitSync.Client;
using BlazorBootstrap;
using OpenGitSync.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using OpenGitSync.Client.Providers;
using OpenGitSync.Client.MessageHandlers;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using Toolbelt.Blazor;

Console.WriteLine("Starting OGS...");

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// if appsettings needed
//var environment = builder.Configuration.GetValue<string>("ASPNETCORE_ENVIRONMENT");
//builder.Configuration.AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);
//builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
//var apiUrl = builder.Configuration.GetValue<string>("ClientSettings:ApiUrl");
//var apiUrl = "https://localhost:51042/";

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazorBootstrap();
builder.Services.AddApiAuthorization();

builder.Services.AddHttpClientInterceptor();

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => new HttpClient(
    sp.GetRequiredService<CustomAuthorizationMessageHandler>())
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
}.EnableIntercept(sp));

// Supply HttpClient instances that does not include access tokens when making requests to the server project
builder.Services.AddHttpClient("OpenGitSync.PublicServerAPI", (sp, client) => { client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress); client.EnableIntercept(sp); });

builder.Services.AddSingleton<ToastService>();

builder.Services.AddScoped<HttpInterceptorService>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAccessTokenProvider, CustomAccessTokenProvider>();

builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<CustomAuthenticationStateProvider>());
builder.Services.AddScoped<CustomAuthorizationMessageHandler>();

builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IProjectService, ProjectService>();
builder.Services.AddTransient<IRepositoryService, RepositoryService>();

var host = builder.Build();

var httpInterceptor = host.Services.GetService<HttpInterceptorService>();

httpInterceptor?.RegisterEvent();

await host.RunAsync();


