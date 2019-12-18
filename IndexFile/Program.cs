using System;
using CommandLine;
using Konsole;

namespace IndexFile
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<IndexerConfig>(args)
                .WithParsed(RunApplicaiton);

            Console.ReadKey();
        }

        public static void RunApplicaiton(IndexerConfig config)
        {
            ProgressBar progress = null;
            IIndexer indexer;

            if (config.UseLucene)
            {
                indexer = new LuceneIndexer();
            }
            else
            {
                indexer = new ElasticIndexer();
            }

            var indexResult = indexer.IndexFiles(config, result =>
            {
                if (progress == null)
                {
                    progress = new ProgressBar(PbStyle.DoubleLine, result.TotalFile, 200);
                }

                progress.Refresh(result.ReadedFiles, result.ToString());
            });

            Console.WriteLine(indexResult);
        }
    }
}