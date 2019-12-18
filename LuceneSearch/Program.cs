using System;
using System.Reflection;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using PdfExtractor;

namespace LuceneSearch
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var appLuceneVersion = Lucene.Net.Util.Version.LUCENE_30;
            var indexLocation = @"D:\Index";
            var directory = FSDirectory.Open(indexLocation);
            var analyzer = new StandardAnalyzer(appLuceneVersion);

            using (var indexSearcher = new IndexSearcher(directory))
            {
                if (args.Length < 2)
                {
                    Console.WriteLine("Usage: lucenesearch <Field Name> <Query> ...");
                    Console.WriteLine("Where field name include: title, content, author, creator");
                }
                else
                {
                    string field = args[0];
                    var queryParser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, field, analyzer);

                    for (var i = 1; i < args.Length; i++)
                    {
                        var term = args[i];
                        Query query = queryParser.Parse(term);

                        TopDocs hits = indexSearcher.Search(query, 10);

                        foreach (ScoreDoc scoreDoc in hits.ScoreDocs)
                        {
                            var document = indexSearcher.Doc(scoreDoc.Doc);

                            var book = GetObject<PdfBook>(document);

                            Console.WriteLine("{0} {1}", scoreDoc.Score, book);
                        }
                    }
                    Console.WriteLine("Search completed.");
                }
            }
            Console.ReadLine();
        }

        private static T GetObject<T>(Document document)
        {
            var properties = typeof(T).GetProperties();

            T instance = (T)Activator.CreateInstance(typeof(T));

            foreach (var property in properties)
            {
                var prop = instance.GetType().GetProperty(property.Name, BindingFlags.Public | BindingFlags.Instance);

                if (prop != null && prop.CanWrite)
                {
                    prop.SetValue(instance, document.Get(property.Name), null);
                }
            }

            return instance;
        }
    }
}