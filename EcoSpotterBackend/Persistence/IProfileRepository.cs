using EcoSpotterBackend.Model;
namespace EcoSpotterBackend.Persistence
{
    public interface IProfileRepository
    {
        public List<Profile> GetAll();
        public Profile GetById(int id);
        public void Add(Profile profile);
        public void Update(Profile profile);
        public void Delete(int id);
    }
}
