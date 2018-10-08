using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Vivid.Web.Extensions;

namespace Vivid.Web
{
    /// <summary>
    /// Contains app's startup tasks
    /// </summary>
    public class Startup
    {
        private IConfiguration Configuration { get; }

        /// <inheritdoc />
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Configure app's IoC container
        /// </summary>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMongoDb(Configuration.GetSection("Data"));

            services.AddOperationServices();

            services.AddAuth();

            services.AddMvc();

            services.AddSwaggerDocs();

            services.AddCors();
        }

        /// <summary>
        /// Configure web app's request pipeline
        /// </summary>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.SeedData();
            }

            app.UseCors(cors => cors
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetPreflightMaxAge(TimeSpan.FromDays(7))
            );

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseSwagger(options => { options.RouteTemplate = "api/docs/swagger/{documentName}/swagger.json"; });
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "api/docs/swagger";
                c.SwaggerEndpoint("v1/swagger.json", "v1");
            });

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
