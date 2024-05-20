
using api.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace api.Data
{
    public class ApplicationDBContext
    {

        private readonly IMongoCollection<Stock> _stockCollection;
        private readonly IMongoCollection<Comment> _commentCollection;
        private readonly IMongoCollection<User> _userCollection;

        public ApplicationDBContext(DatabaseSettings settings)
        {
            var mongoClient = new MongoClient(settings.ConnectionString);
            var mongoDb = mongoClient.GetDatabase(settings.DatabaseName);
            _stockCollection = mongoDb.GetCollection<Stock>(settings.StockCollectionName);
            _commentCollection = mongoDb.GetCollection<Comment>(settings.CommentCollectionName);
            _userCollection = mongoDb.GetCollection<User>(settings.UserCollectionName);
        }

        public IMongoCollection<Stock> StockCollection => _stockCollection;
        public IMongoCollection<Comment> CommentCollection => _commentCollection;
        public IMongoCollection<User> UserCollection => _userCollection;

    }
}