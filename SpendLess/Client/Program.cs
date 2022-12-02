global using Blazored.LocalStorage;
global using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;

using SpendLess.Client;
using SpendLess.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
var dir = Environment.CurrentDirectory;
builder.Services.AddMudServices();
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddScoped<AuthenticationService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IBalanceService, BalanceService>();
builder.Services.AddAuthorizationCore();
builder.Services.AddHttpClient("API", c =>
{
    c.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
});
builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();
