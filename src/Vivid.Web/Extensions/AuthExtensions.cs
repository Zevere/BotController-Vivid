using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Vivid.Web.Middlewares.BasicAuth;

namespace Vivid.Web.Extensions
{
    internal static class AuthExtensions
    {
        /// <summary>
        /// Adds authorization services to the app's service collection
        /// </summary>
        public static IServiceCollection AddAuth(this IServiceCollection services)
        {
            services.AddAuthentication("Basic")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("Basic", null);

            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicy(
                    new[]
                    {
                        new AssertionRequirement(authContext => authContext.User.FindFirstValue("token") != null)
                    },
                    new[] { "Basic" }
                );
            });

            return services;
        }
    }
}
