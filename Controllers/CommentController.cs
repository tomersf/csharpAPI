using api.Dtos.Comment;
using api.Mappers;
using api.Models;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/comments")]
    public class CommentController : ControllerBase
    {
        private readonly CommentService _commentServie;
        public CommentController(CommentService commentService)
        {
            _commentServie = commentService;
        }

        [HttpGet]
        public async Task<List<Comment>> Get() => await _commentServie.GetCommentsAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Comment>> Get(string id)
        {
            Comment comment = await _commentServie.GetCommentAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment);
        }

        [HttpPost]
        public async Task<ActionResult<Comment>> Post([FromBody] CreateCommentDto commentDto)
        {

            var commentModel = commentDto.ToCommentFromCreate(commentDto.StockId);
            if (commentModel.StockId == string.Empty)
            {
                return BadRequest("StockId is required");
            }
            await _commentServie.CreateCommentAsync(commentModel);
            return CreatedAtAction(nameof(Get), new { id = commentModel.Id }, commentModel.ToCommentDto());
        }

        [HttpPut("{id:length(24)}")]
        public async Task<ActionResult> Put(string id, [FromBody] Comment updateComment)
        {
            Comment comment = await _commentServie.GetCommentAsync(id);
            if (comment == null)
            {
                return NotFound("There is no comment with this id");
            }

            updateComment.Id = comment.Id;
            await _commentServie.UpdateCommentAsync(id, updateComment);

            return Ok("Updated successfully");
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<ActionResult> Delete(string id)
        {
            Comment comment = await _commentServie.GetCommentAsync(id);
            if (comment == null)
            {
                return NotFound("There is no comment with this id " + id);
            }

            await _commentServie.DeleteCommentAsync(id);

            return Ok("Deleted successfully");
        }
    }
}