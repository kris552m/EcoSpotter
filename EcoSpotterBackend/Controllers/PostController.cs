using Microsoft.AspNetCore.Mvc;
using EcoSpotterBackend.Model;
using EcoSpotterBackend.Persistence;
namespace EcoSpotterBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository repo;
        public PostController(IPostRepository repo) {
            this.repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        [HttpPost]
        public ActionResult<Post> Add([FromBody] Post post)
        {
            repo.Add(post);
            return CreatedAtAction(nameof(GetById), new { id = post.Id }, post);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var existingPost = repo.GetById(id);
            if (existingPost == null) return NotFound();
            repo.Delete(id);
            return NoContent();
        }

        [HttpGet]
        public ActionResult<List<Post>> GetAll()
        {
            return Ok(repo.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<Post> GetById(int id)
        {

            var post = repo.GetById(id);
            return post is null ? NotFound() : Ok(post);
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] Post post)
        {
            if (post == null || post.Id != id) return BadRequest();
            var existingPost = repo.GetById(id);
            if (existingPost == null) return NotFound();
            repo.Update(post);
            return NoContent();
        }
    }
}
