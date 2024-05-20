using api.Data;
using api.Helpers;
using api.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace api.Services
{
    public class StockService
    {
        private readonly IMongoCollection<Stock> _stockCollection;

        public StockService(ApplicationDBContext dbContext)
        {
            _stockCollection = dbContext.StockCollection;
        }

        public async Task<List<Stock>> GetStocksAsync(QueryObject query)
        {
            var pipeline = new List<BsonDocument>
                            {
                                buildCommentLookupDoc()
                            };

            if (!string.IsNullOrWhiteSpace(query.CompanyName))
            {
                var matchStage = new BsonDocument("$match", new BsonDocument("companyname", query.CompanyName));
                pipeline.Insert(0, matchStage);
            }

            return await _stockCollection.Aggregate<Stock>(pipeline).ToListAsync();
        }


        public async Task<Stock> GetStockAsync(string id)
        {
            var pipeline = new[]
            {
                buildStockMatchDoc(new ObjectId(id)),
                buildCommentLookupDoc()
            };
            return await _stockCollection.Aggregate<Stock>(pipeline).FirstOrDefaultAsync();
        }


        public async Task CreateStockAsync(Stock stock) =>
            await _stockCollection.InsertOneAsync(stock);


        public async Task<ReplaceOneResult> UpdateStockAsync(string id, Stock stock) =>
            await _stockCollection.ReplaceOneAsync(s => s.Id == id, stock);

        public async Task<DeleteResult> DeleteStockAsync(string id) =>
            await _stockCollection.DeleteOneAsync(s => s.Id == id);


        private BsonDocument buildStockMatchDoc(ObjectId id) => new BsonDocument("$match", new BsonDocument("_id", id));

        private BsonDocument buildCommentLookupDoc() => new BsonDocument("$lookup", new BsonDocument
                {
                    { "from", "comments" },
                    { "localField", "_id" },
                    { "foreignField", "stockId" },
                    { "as", "comments" }
                });
    }
}