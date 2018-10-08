using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Vivid.Web.Options;
using Zevere.Client;

namespace Vivid.Web.Extensions
{
    internal static class ZevereClientExtensions
    {
        /// <summary>
        /// Adds client for Zevere GraphQL API to the app's service collection
        /// </summary>
        public static void AddZevereClient(
            this IServiceCollection services,
            IConfigurationSection zevereSection
        )
        {
            string endpoint = zevereSection.GetValue<string>(nameof(ZevereOptions.Endpoint));
            if (!Uri.TryCreate(endpoint ?? "", UriKind.RelativeOrAbsolute, out _))
            {
                throw new ArgumentException($@"Invalid Zevere GraphQL endpoint: ""{endpoint}"".");
            }

            services.Configure<ZevereOptions>(zevereSection);

            services.AddScoped<IZevereClient, ZevereClient>(
                provider => new ZevereClient(
                    endpoint,
                    provider.GetRequiredService<ILogger<ZevereClient>>()
                )
            );
        }
    }
}
