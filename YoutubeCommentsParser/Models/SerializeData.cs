using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeCommentsParser.Models
{
    [Serializable]
    public class SerializeData
    {
        public string YoutubeKey { get; set; }
        public Dictionary<Guid, SearchProject> Projects { get; set; }
    }
}
