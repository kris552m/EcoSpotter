using System;
using System.Collections.Generic;
using System.Text;

namespace EcoSpotterPostPrototype.Model
{
    public class PostImage
    {
        public PostImage(string beforeImageUrl, string afterImageUrl)
        { 
            BeforeImageUrl = beforeImageUrl;
            AfterImageUrl = afterImageUrl;
        }
        public int Id { get; set; }
        public Post Post { get; set; }
        public int PostId { get; set; }
        public string BeforeImageUrl { get; set; }
        public string AfterImageUrl { get; set; }

    }
}
