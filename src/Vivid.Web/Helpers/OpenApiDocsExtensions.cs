using System.IO;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace Vivid.Web.Helpers
{
    internal static class OpenApiDocsExtensions
    {
        public static IServiceCollection AddOpenAPIDocs(this IServiceCollection services)
        {
            string thisAssembly = Assembly.GetCallingAssembly().GetName().Name;
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info {Title = "Bot Ops - Vivid", Version = "v1"});

                var filePath = Path.Combine(System.AppContext.BaseDirectory, $"{thisAssembly}.xml");
                c.IncludeXmlComments(filePath);
            });

            return services;
        }
    }
}