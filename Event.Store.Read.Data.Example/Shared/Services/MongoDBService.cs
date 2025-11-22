using MongoDB.Driver;
using Shared.Services.Abstractions;

namespace Shared.Services
{
    public class MongoDBService : IMongoDBService
    {
        private readonly IMongoDatabase _database;

        public MongoDBService(string connectionString = "mongodb://localhost:27017", string databaseName = "ProductDB")
        {
            var mongoClient = new MongoClient(connectionString);
            _database = mongoClient.GetDatabase(databaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }
    }
}