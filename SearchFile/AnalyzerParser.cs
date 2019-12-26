using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Util;

namespace SearchFile
{
    public class AnalyzerParser
    {
        public static Analyzer Parse(string name, Version version = Version.LUCENE_30)
        {
            return new StandardAnalyzer(version);
        }
    }
}