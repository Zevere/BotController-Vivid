using System;
using Vivid.Web.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Threading.Tasks;
using Vivid.Web.Extensions;
using Vivid.Web.Middlewares.BasicAuth;

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

            #region Auth

            services.AddAuthentication("Basic")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("Basic", default);

            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicy(
                    new[]
                    {
                        new AssertionRequirement(authContext => authContext.User.FindFirstValue("token") != default)
                    },
                    new[] { "Basic" });
            });

            #endregion

            services.AddMvc();

            services.AddOpenAPIDocs();

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

//            app.UseAuthentication();

            app.UseMvc();

            app.UseSwagger(options => { options.RouteTemplate = "api/docs/swagger/{documentName}/swagger.json"; });
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "api/docs/swagger";
                c.SwaggerEndpoint("v1/swagger.json", "v1");
            });

            app.Run(context =>
            {
                context.Response.Redirect("/api/docs/swagger");
                return Task.CompletedTask;
            });
        }
    }
}
