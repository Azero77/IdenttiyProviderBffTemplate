using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using App.Client.Services;

namespace App.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            //builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddHttpClient("apiClient", client =>
            {
                client.BaseAddress = new Uri("https://localhost:5002/");
            }).AddHttpMessageHandler<AntiforgeryHandler>()
            .AddHttpMessageHandler<CookieHandler>();
            builder.Services.AddSingleton<CookieHandler>();
            builder.Services.AddSingleton<AntiforgeryHandler>();
            builder.Services.AddSingleton<HttpClientErrorDelegationHandler>();
            builder.Services.AddAuthorizationCore();

            builder.Services.AddSingleton<AuthenticationStateProvider, BffAuthenticationStateProvider>();
            await builder.Build().RunAsync();
        }
    }
}
