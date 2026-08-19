using EcoSpotterBackend.Model;
namespace EcoSpotterBackend.Persistence
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly ecoDBContext _context;
        public ProfileRepository(ecoDBContext _context)
        {
            this._context = _context;
            //Profiles.Add(new Profile { Id = 1, Name = "Kristoffer" });
            //Profiles.Add(new Profile { Id = 2, Name = "Mads" });
            //Profiles.Add(new Profile { Id = 3, Name = "Mikkel" });
        }
        public List<Profile> GetAll()
        {
            return _context.Profiles.ToList();
        }
        public Profile GetById(int id)
        {
            return (Profile)_context.Posts.Where(x => x.Id == id);
        }
        public void Add(Profile profile)
        {
            _context.Add(profile);
        }
        public void Update(Profile profile)
        {
            _context.Update(profile);
        }
        public void Delete(int id)
        {
            _context.Remove(GetById(id));
        }
    }
}
