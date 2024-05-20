
using api.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace api.Data
{
    public class ApplicationDBContext
    {

        private readonly IMongoCollection<Stock> _stockCollection;
        private readonly IMongoCollection<Comment> _commentCollection;

        public ApplicationDBContext(DatabaseSettings settings)
        {
            var mongoClient = new MongoClient(settings.ConnectionString);
            var mongoDb = mongoClient.GetDatabase(settings.DatabaseName);
            _stockCollection = mongoDb.GetCollection<Stock>(settings.StockCollectionName);
            _commentCollection = mongoDb.GetCollection<Comment>(settings.CommentCollectionName);
        }

        public IMongoCollection<Stock> StockCollection => _stockCollection;
        public IMongoCollection<Comment> CommentCollection => _commentCollection;

    }
}