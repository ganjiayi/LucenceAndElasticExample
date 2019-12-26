using System.Collections.Generic;

namespace SearchFile
{
    public class SearchConfig
    {
        public IEnumerable<string> ObjectConfig { get; set; }

        public IEnumerable<string> Terms { get; set; }

        public string Field { get; set; }

        public bool UseLucene { get; set; }

        public string IndexedPath { get; set; }

        public int NumberOfHits { get; set; }

        public string Analyzer { get; set; }
    }
}