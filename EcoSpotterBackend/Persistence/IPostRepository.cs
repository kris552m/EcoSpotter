using EcoSpotterBackend.Model;
namespace EcoSpotterBackend.Persistence
{
    public interface IPostRepository
    {
        public List<Post> GetAll();
        public Post GetById(int id);
        public void Add(Post post);
        public void Update(Post post);
        public void Delete(int id);
    }
}
