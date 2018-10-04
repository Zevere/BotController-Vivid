using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using Vivid.Data.Abstractions;
using Vivid.Data.Abstractions.Entities;
using Vivid.Data.Mongo;
using Vivid.Data.Mongo.Entities;
using UserEntity = Vivid.Data.Abstractions.Entities.User;

namespace Vivid.Web.Helpers
{
    internal static class MongoExtensions
    {
        public static IServiceCollection AddMongo(this IServiceCollection services, string connectionString)
        {
            string dbName = new ConnectionString(connectionString).DatabaseName;
            services.AddSingleton<IMongoClient, MongoClient>(_ => new MongoClient(connectionString));
            services.AddTransient<IMongoDatabase>(_ => _.GetRequiredService<IMongoClient>().GetDatabase(dbName));

            services.AddTransient<IMongoCollection<User>>(_ =>
                _.GetRequiredService<IMongoDatabase>()
                    .GetCollection<User>(MongoConstants.Collections.Users.Name)
            );
            services.AddTransient<IMongoCollection<TaskListMongo>>(_ =>
                _.GetRequiredService<IMongoDatabase>()
                    .GetCollection<TaskListMongo>(MongoConstants.Collections.TaskLists.Name)
            );
            services.AddTransient<IMongoCollection<TaskItemMongo>>(_ =>
                _.GetRequiredService<IMongoDatabase>()
                    .GetCollection<TaskItemMongo>(MongoConstants.Collections.TaskItems.Name)
            );
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ITaskListRepository, TaskListRepository>();
            services.AddTransient<ITaskItemRepository, TaskItemRepository>();
            
            Initializer.RegisterClassMaps();
            
            return services;
        }
    }
}