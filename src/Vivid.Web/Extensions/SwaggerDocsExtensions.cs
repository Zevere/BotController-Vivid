using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace Vivid.Web.Extensions
{
    internal static class OpenApiDocsExtensions
    {
        /// <summary>
        /// Adds Open API (Swagger) services to the app's service collection
        /// </summary>
        public static void AddSwaggerDocs(this IServiceCollection services)
        {
            string thisAssembly = Assembly.GetCallingAssembly().GetName().Name;
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Bot Ops - Vivid", Version = "v1" });

                // Load XML comments
                var filePath = Path.Combine(System.AppContext.BaseDirectory, $"{thisAssembly}.xml");
                c.IncludeXmlComments(filePath);

                // Security definitions
                c.AddSecurityDefinition("basic", new BasicAuthScheme
                {
                    Description = "API uses Basic HTTP authentication"
                });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { "basic", new[] { "" } }
                });
            });
        }
    }
}
