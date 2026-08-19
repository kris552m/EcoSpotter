using System;
using System.Collections.Generic;
using System.Text;

namespace EcoSpotterBackend.Model;

public class Post
{
    int idToBe = 3;
    public Post() { }
    public Post(int UserId, string Description, PostImage Images)
    {
        this.UserId = UserId;
        this.Description = Description;
        this.PostImage = Images;
        this.Id = idToBe;

        if (PostImage != null)
        {
            PostImage.Post = this;
        }
    }

    private int id;
    public int Id
    {
        get { return id; }
        set { id = idToBe; idToBe++; }
    }

    public int UserId { get; set; }
    public DateTime DateCreated { get; set; }
    public string Description { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Location { get; set; }
    public PostImage PostImage { get; set; }
    public int PostImageId { get; set; }
    public string FormattedDate => DateCreated.ToString("MMMM dd, yyyy");
    public Profile? AuthorProfile { get; set; }
}
