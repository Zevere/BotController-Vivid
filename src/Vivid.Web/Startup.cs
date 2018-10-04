﻿using System;
using Vivid.Web.Data;
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
using Microsoft.AspNetCore.Http;
using Vivid.Web.Middlewares.BasicAuth;

namespace Vivid.Web
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            #region Database

            string dataStore = Configuration["data:use"];
            if (dataStore == "mongo")
            {
                string connStr = Configuration["data:mongo:connection"];
                services.AddMongo(connStr);
            }

            #endregion

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
                    new[] {"Basic"});
            });

            #endregion

            services.AddMvc();

            services.AddOpenAPIDocs();

            services.AddCors();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILogger<Startup> logger)
        {
            logger.LogInformation($@"Using database ""{Configuration["data:use"]}"".");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
//                app.SeedData(Configuration.GetSection("data"));
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

            app.Run(context => context.Response.WriteAsync("Hello, World! Welcome to Borzoo ;)"));
        }
    }
}