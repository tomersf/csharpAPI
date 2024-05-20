using api.Data;
using api.Models;
using MongoDB.Driver;

namespace api.Services
{
    public class CommentService
    {
        private readonly IMongoCollection<Comment> _commentCollection;

        public CommentService(ApplicationDBContext dbContext)
        {
            _commentCollection = dbContext.CommentCollection;
        }

        public async Task<List<Comment>> GetCommentsAsync() => await _commentCollection.Find(stock => true).ToListAsync();

        public async Task<Comment> GetCommentAsync(string id) =>
            await _commentCollection.Find(stock => stock.Id == id).FirstOrDefaultAsync();


        public async Task CreateCommentAsync(Comment stock) =>
            await _commentCollection.InsertOneAsync(stock);


        public async Task<ReplaceOneResult> UpdateCommentAsync(string id, Comment stock) =>
            await _commentCollection.ReplaceOneAsync(s => s.Id == id, stock);

        public async Task<DeleteResult> DeleteCommentAsync(string id) =>
            await _commentCollection.DeleteOneAsync(s => s.Id == id);
    }
}