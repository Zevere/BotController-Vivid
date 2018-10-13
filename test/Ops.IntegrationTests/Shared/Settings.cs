using System.IO;
using Microsoft.Extensions.Configuration;

namespace Ops.IntegrationTests.Shared
{
    public class Settings
    {
        public string MongoConnection { get; }

        public string ZevereApiEndpoint { get; }

        public Settings()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("settings.json")
                .AddJsonEnvVar("VIVID_TEST_SETTINGS", optional: true)
                .Build();

            MongoConnection = configuration[nameof(MongoConnection)];
            ZevereApiEndpoint = configuration[nameof(ZevereApiEndpoint)];
        }
    }
}
