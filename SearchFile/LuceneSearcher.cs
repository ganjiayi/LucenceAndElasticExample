using System.Collections.Generic;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;

namespace SearchFile
{
    public class LuceneSearcher : ISearcher
    {
        private readonly SearchConfig _searchConfig;
        private readonly FSDirectory _directory;
        private readonly Lucene.Net.Util.Version _version;
        private readonly Analyzer _analyzer;

        public LuceneSearcher(SearchConfig searchConfig)
        {
            _searchConfig = searchConfig;
            _version = Lucene.Net.Util.Version.LUCENE_30;
            _directory = FSDirectory.Open(searchConfig.IndexedPath);
            _analyzer = AnalyzerParser.Parse(searchConfig.Analyzer);
        }

        public IList<DynamicClass> Search(string fieldName, string term)
        {
            using (var searcher = new IndexSearcher(_directory))
            {
                var queryParser = new QueryParser(_version, fieldName, _analyzer);
                var query = queryParser.Parse(term);
                var hits = searcher.Search(query, _searchConfig.NumberOfHits);

                foreach (var scoreDoc in hits.ScoreDocs)
                {
                    var document = searcher.Doc(scoreDoc.Doc);
                }
            }
        }

        public IList<DynamicClass> Search<NumberType>(string fieldName, NumberType min, NumberType max)
        {
            throw new System.NotImplementedException();
        }
    }
}