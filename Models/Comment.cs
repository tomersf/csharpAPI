using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api.Models
{
    public class Comment
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("title")]
        public string Title { get; set; } = string.Empty;

        [BsonElement("content")]
        public string Content { get; set; } = string.Empty;

        [BsonElement("createdOn")]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [BsonElement("stockId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string StockId { get; set; } = string.Empty;
    }
}