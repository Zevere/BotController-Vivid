using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Vivid.Data.Abstractions;
using Vivid.Data.Abstractions.Entities;
using Vivid.Data.Mongo;
using Vivid.Web.Options;

namespace Vivid.Web.Extensions
{
    internal static class DataSeederExtensions
    {
        public static void SeedData(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Startup>>();
                logger.LogInformation("Initializing MongoDB instance...");

                var db = scope.ServiceProvider.GetRequiredService<IMongoDatabase>();
                bool dbInitialized = InitMongoDbAsync(db).GetAwaiter().GetResult();
                if (dbInitialized)
                    logger.LogInformation("MongoDB instance is initialized.");

                var botRepo = scope.ServiceProvider.GetRequiredService<IChatBotRepository>();
                bool seeded = SeedData(botRepo).GetAwaiter().GetResult();
                logger.LogInformation($"Database is{(seeded ? "" : " NOT")} seeded.");
            }
        }

        private static async Task<bool> SeedData(IChatBotRepository botRepo)
        {
            if (await IsAlreadySeeded(botRepo))
            {
                return false;
            }

            ChatBot[] testBots =
            {
                new ChatBot
                {
                    Name = "tg-bot",
                    Platform = "telegram",
                    Url = "https://localhsot:5001/tg-bot",
                    Token = "foo:bar",
                },
                new ChatBot
                {
                    Name = "zvOnSlack",
                    Platform = "slack",
                    Url = "https://localhsot:5001/zvOnSlack",
                    Token = "aT0k3nFoR:zvOnSlack",
                },
            };

            foreach (var bot in testBots)
            {
                await botRepo.AddAsync(bot);
            }

            return true;
        }

        private static async Task<bool> IsAlreadySeeded(IChatBotRepository botRepo)
        {
            var bot = await botRepo.GetByNameAsync("tg-bot");
            bool botExists = bot != null;
            return botExists;
        }

        private static async Task<bool> InitMongoDbAsync(IMongoDatabase db)
        {
            var curser = await db.ListCollectionsAsync();
            if (curser.MoveNext() && curser.Current.Any())
            {
                return false;
            }

            await Initializer.CreateSchemaAsync(db);
            return true;
        }
    }
}
