using Microsoft.Extensions.DependencyInjection;
using Vivid.Ops;

namespace Vivid.Web.Extensions
{
    internal static class OpsExtensions
    {
        /// <summary>
        /// Adds operations services to the app's service collection
        /// </summary>
        public static void AddOperationServices(
            this IServiceCollection services
        )
        {
            services.AddScoped<IRegistrationService, RegistrationService>();
            services.AddScoped<IUserProfileService, UserProfileService>();
        }
    }
}
