using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EcoSpotterBackend.Model;

public class Profile
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ProfilePictureUrl { get; set; } = string.Empty;
    public List<Post> Posts { get; set; }
}