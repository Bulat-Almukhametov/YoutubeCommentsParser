using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeCommentsParser.Models
{
    public struct WordsTonality
    {
        public string Word { get; set; }
        public int Tonality { get; set; }
    }

    public sealed class WordsTonalityMap : ClassMap<WordsTonality>
    {
        public WordsTonalityMap()
        {
            Map(m => m.Word).Index(0);
            Map(m => m.Tonality).Index(1);
        }
    }
}
