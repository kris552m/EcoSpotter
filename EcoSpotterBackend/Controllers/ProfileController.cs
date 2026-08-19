using Microsoft.AspNetCore.Mvc;
using EcoSpotterBackend.Model;
using EcoSpotterBackend.Persistence;

namespace EcoSpotterBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileRepository repo;
        public ProfileController(IProfileRepository repo)
        {
            this.repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        [HttpPost]
        public ActionResult<Profile> Add([FromBody] Profile profile)
        {
            repo.Add(profile);
            return CreatedAtAction(nameof(GetById), new { id = profile.Id }, profile);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var existingProfile = repo.GetById(id);
            if (existingProfile == null) return NotFound();
            repo.Delete(id);
            return NoContent();
        }

        [HttpGet]
        public ActionResult<List<Profile>> GetAll()
        {
            return Ok(repo.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<Profile> GetById(int id)
        {
            var profile = repo.GetById(id);
            return profile is null ? NotFound() : Ok(profile);
        }

        [HttpPut]
        public ActionResult Update(int id, [FromBody] Profile profile)
        {
            if (profile == null || profile.Id != id) return BadRequest();
            var existingProfile = repo.GetById(id);
            if (existingProfile == null) return NotFound();
            repo.Update(profile);
            return NoContent();
        }
    }
}
