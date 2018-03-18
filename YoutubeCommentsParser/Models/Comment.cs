using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeCommentsParser.Models
{
    [Serializable]
    public struct Comment
    {
        public string Content { get; set; }
        public int Rate { get; set; }
    }
}
