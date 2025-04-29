using Duende.Bff.Yarp;
using Serilog;
using App.Shared;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.HttpResults;

namespace App.Bff
{
    internal static class HostingExtensions
    {
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddRazorPages();

            builder.Services.AddControllers();

            // add BFF services and server-side session management
            builder.Services.AddBff()
                // if you wanted to enable a remote API (in addition or instead of the local API), then you could uncomment this line
                .AddRemoteApis()
                .AddServerSideSessions();
            builder.Services.AddCors(setup => 
            {
                setup.AddPolicy("SPAClient", p =>
                {
                    p.WithOrigins(GlobalConfiguration.ClientUri)
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .AllowAnyHeader();
                });
            });
            builder.Services.AddAuthentication(options =>
                {
                    options.DefaultScheme = "cookie";
                    options.DefaultChallengeScheme = "oidc";
                    options.DefaultSignOutScheme = "oidc";
                })
                .AddCookie("cookie", options =>
                {
                    options.Cookie.Name = "__Host-bff";
                    options.Cookie.SameSite = SameSiteMode.Lax;
                    options.SlidingExpiration = true;
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.ExpireTimeSpan = TimeSpan.FromHours(8);
                })
                .AddOpenIdConnect("oidc", options =>
                {
                    options.Authority = GlobalConfiguration.IdentityUri;
                    options.ClientId = "interactive.confidential";
                    options.ClientSecret = "secret";
                    options.ResponseType = "code";
                    options.ResponseMode = "query";

                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.SaveTokens = true;
                    options.MapInboundClaims = false;

                    options.Scope.Clear();
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                    options.Scope.Add("webApi");
                    options.Scope.Add("offline_access");

                    options.TokenValidationParameters.NameClaimType = "name";
                    options.TokenValidationParameters.RoleClaimType = "role";
                });

            return builder.Build();
        }

        public static WebApplication ConfigurePipeline(this WebApplication app)
        {
            app.UseSerilogRequestLogging();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors("SPAClient");
            app.UseAuthentication();

            // add CSRF protection and status code handling for API endpoints
            app.UseBff();
            app.UseAuthorization();

            // local API endpoints
            app.MapControllers()
                .RequireAuthorization()
                .AsBffApiEndpoint();

            app.MapBffManagementEndpoints();

            // if you wanted to enable a remote API (in addition or instead of the local API), then you could uncomment these lines
            app.MapRemoteBffApiEndpoint("/api", $"{GlobalConfiguration.ApiUri}/api/")
            .RequireAccessToken(Duende.Bff.TokenType.User);

            app.MapGet("/",  (context) => {
                context.Response.Redirect(GlobalConfiguration.ClientUri);
                return Task.CompletedTask;
            }) ;

            app.MapGet("/accessToken", async (HttpContext context) => {
                var token = await context.GetUserAccessTokenAsync();
                return token;
            });
            return app;
        }
    }
}
