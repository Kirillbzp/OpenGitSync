using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using OpenGitSync.Client;
using BlazorBootstrap;
using OpenGitSync.Client.Services;

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

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddHttpClient("OpenGitSync.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

builder.Services.AddHttpClient("OpenGitSync.PublicServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("OpenGitSync.ServerAPI"));

builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazorBootstrap();
builder.Services.AddApiAuthorization();

await builder.Build().RunAsync();
