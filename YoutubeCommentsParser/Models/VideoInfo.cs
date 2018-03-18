using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeCommentsParser.Models
{
    [Serializable]
    public struct VideoInfo
    {
        public string Id { get; set; }
        public string Caption { get; set; }
        public int ViewedCount { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
