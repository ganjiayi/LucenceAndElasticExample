using System;
using Nest;
using PdfExtractor;

namespace ElasticSearch
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var settings = new ConnectionSettings(new Uri("http://localhost:9200")).DefaultIndex("book");
            var client = new ElasticClient(settings);

            if (args.Length < 2)
            {
                Console.WriteLine("Usage: elasticsearch <Field Name> <Query> ...");
                Console.WriteLine("Where field name include: title, content, author, creator");
            }
            else
            {
                string field = args[0];

                for (var i = 1; i < args.Length; i++)
                {
                    var searchRequest = new SearchRequest<PdfBook>
                    {
                        Query = new MatchQuery
                        {
                            Field = field,
                            Query = args[i]
                        }
                    };

                    var searchResult = client.Search<PdfBook>(searchRequest);

                    var documents = searchResult.Documents;

                    foreach (var document in documents)
                    {
                        Console.WriteLine(document);
                    }

                    Console.WriteLine("Document count: " + documents.Count);
                }
            }

            Console.ReadKey();
        }
    }
}