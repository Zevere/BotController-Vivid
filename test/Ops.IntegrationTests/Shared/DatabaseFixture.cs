using System;
using System.Diagnostics;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using Vivid.Data;
using Vivid.Data.Entities;

namespace Ops.IntegrationTests.Shared
{
    public class DatabaseFixture
    {
        public IMongoCollection<ChatBot> ChatBotsCollection =>
            _mongoDatabase.GetCollection<ChatBot>("bots");

        public IMongoCollection<Registration> RegistrationsCollection =>
            _mongoDatabase.GetCollection<Registration>("registrations");

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

        /// <summary>
        /// Initializes the MongoDB database. Recreates the database, if exists, and creates its schema
        /// </summary>
        private static async Task<IMongoDatabase> InitializeDatabase()
        {
            var settings = new Settings();
            var connectionString = new ConnectionString(settings.MongoConnection);

            var clientSettings = MongoClientSettings.FromConnectionString(settings.MongoConnection);
            clientSettings.ClusterConfigurator = ClientSettingsClusterConfigurator;

            var client = new MongoClient(clientSettings);

            await client.DropDatabaseAsync(connectionString.DatabaseName);
            var db = client.GetDatabase(connectionString.DatabaseName);

            await Initializer.CreateSchemaAsync(db);

            return db;
        }

        /// <summary>
        /// Configures a MongoDB client to write its logs to the standard output
        /// </summary>
        private static void ClientSettingsClusterConfigurator(ClusterBuilder cb)
        {
            var traceSource = new TraceSource("~~ MongoDB ~~", SourceLevels.Warning);
            traceSource.Listeners.Clear();
            var listener = new TextWriterTraceListener(Console.Out) { TraceOutputOptions = TraceOptions.DateTime, };
            traceSource.Listeners.Add(listener);
            cb.TraceWith(traceSource);
        }
    }
}
