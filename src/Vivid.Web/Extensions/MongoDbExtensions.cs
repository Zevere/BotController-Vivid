﻿using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using Vivid.Data.Abstractions;
using Vivid.Data.Abstractions.Entities;
using Vivid.Data.Mongo;
using Vivid.Web.Options;

namespace Vivid.Web.Extensions
{
    internal static class MongoDbExtensions
    {
        /// <summary>
        /// Adds MongoDB services to the app's service collections
        /// </summary>
        public static IServiceCollection AddMongoDb(
            this IServiceCollection services,
            IConfigurationSection dataSection
        )
        {
            string connectionString = dataSection.GetValue<string>(nameof(DataOptions.Connection));
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException($@"Invalid MongoDB connection string: ""{connectionString}"".");
            }

            services.Configure<DataOptions>(dataSection);

            string dbName = new ConnectionString(connectionString).DatabaseName;
            services.AddSingleton<IMongoClient, MongoClient>(_ => new MongoClient(connectionString));
            services.AddTransient<IMongoDatabase>(provider =>
                provider.GetRequiredService<IMongoClient>().GetDatabase(dbName)
            );

            services.AddTransient<IMongoCollection<ChatBot>>(provider =>
                provider.GetRequiredService<IMongoDatabase>()
                    .GetCollection<ChatBot>(MongoConstants.Collections.Bots.Name)
            );

            services.AddTransient<IChatBotRepository, ChatBotRepository>();

            Initializer.RegisterClassMaps();

            return services;
        }
    }
}
