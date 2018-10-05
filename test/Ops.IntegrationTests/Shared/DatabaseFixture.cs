using System;
using System.Diagnostics;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using Vivid.Data.Abstractions.Entities;
using Vivid.Data.Mongo;
using Vivid.Data.Mongo.Entities;

namespace Ops.IntegrationTests.Shared
{
    public class DatabaseFixture
    {
        public IMongoCollection<ChatBot> ChatBotsCollection =>
            _mongoDatabase.GetCollection<ChatBot>("bots");

        public IMongoCollection<RegistrationMongo> RegistrationsCollection =>
            _mongoDatabase.GetCollection<RegistrationMongo>("registrations");

        private readonly IMongoDatabase _mongoDatabase;

        /// <summary>
        /// Registers mapping from entity class types to MongoDB schema
        /// </summary>
        static DatabaseFixture()
        {
            Initializer.RegisterClassMaps();
        }

        public DatabaseFixture()
        {
            _mongoDatabase = InitializeDatabase().GetAwaiter().GetResult();
        }

        private static async Task<IMongoDatabase> InitializeDatabase()
        {
            var settings = new Settings();
            var connectionString = new ConnectionString(settings.Connection);

            var clientSettings = MongoClientSettings.FromConnectionString(settings.Connection);
            clientSettings.ClusterConfigurator = cb =>
            {
                var traceSource = new TraceSource("mongodb-tests", SourceLevels.Warning);
                traceSource.Listeners.Clear();
                var listener = new TextWriterTraceListener(Console.Out)
                {
                    TraceOutputOptions = TraceOptions.DateTime,
                };
                traceSource.Listeners.Add(listener);
                cb.TraceWith(traceSource);
            };

            var client = new MongoClient(clientSettings);

            await client.DropDatabaseAsync(connectionString.DatabaseName);
            var db = client.GetDatabase(connectionString.DatabaseName);

            await Initializer.CreateSchemaAsync(db);

            return db;
        }
    }
}
