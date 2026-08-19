using EcoSpotterBackend.Model;

namespace EcoSpotterBackend.Persistence
{
    public class PostRepository : IPostRepository
    {
        private readonly ecoDBContext _context;

        public PostRepository(ecoDBContext _context) 
        {
            this._context = _context;
            //_context.Posts.Add(new Post { Id = 1, UserId = 1, DateCreated = DateTime.Now, Description = "Post 1", Latitude = 55.6761, Longitude = 12.5683, Location = "Copenhagen", Images = new List<string> { "image1.jpg", "image2.jpg" } });
            //_context.Posts.Add(new Post { Id = 2, UserId = 2, DateCreated = DateTime.Now, Description = "Post 2", Latitude = 50.6761, Longitude = 10.5683, Location = "Odense", Images = new List<string> { "image3.jpg", "image4.jpg" } });
        }
        public List<Post> GetAll()
        {
            return _context.Posts.ToList();
        }
        public Post? GetById(int id)
        {
            return (Post)_context.Posts.Where(x => x.Id == id) ?? null;
        }
        public void Add(Post post)
        {
            _context.Posts.Add(post);
        }
        public void Update(Post post)
        {
            _context.Posts.Update(post);
        }
        public void Delete(int id)
        {
            _context.Posts.Remove(GetById(id));
        }
    }
}
