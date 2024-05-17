using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;

namespace fastfood_payment.Infra.MongoDb.Context
{
    [ExcludeFromCodeCoverage]
    public class MongoDbContext : IDisposable
    {
        private readonly IMongoDatabase _database;
        private readonly MongoClient _client;

        public MongoDbContext(IConfiguration config)
        {
            string connectionString = config.GetConnectionString("MongoConnection") ?? throw new ArgumentNullException("Null connection");
            string database = config.GetConnectionString("MongoDatabase") ?? throw new ArgumentNullException("Null database");

            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase(database);
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }

        public async Task<IClientSessionHandle> StartSessionHandler()
        {
            return await _client.StartSessionAsync();
        }

        public void Dispose() => GC.SuppressFinalize(this);
    }
}
