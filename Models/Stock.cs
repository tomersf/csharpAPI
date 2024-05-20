using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api.Models
{
    public class Stock
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("symbol")]
        public string Symbol { get; set; } = string.Empty;

        [BsonElement("companyname")]
        public string CompanyName { get; set; } = string.Empty;

        [BsonElement("price")]
        public int Purchase { get; set; }

        [BsonElement("lastdiv")]
        public int LastDiv { get; set; }

        [BsonElement("industry")]
        public string Industry { get; set; } = string.Empty;

        [BsonElement("marketcap")]
        public long MarketCap { get; set; }

        [BsonElement("comments")]
        [BsonIgnoreIfNull]
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }

}