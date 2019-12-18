using System;
using System.Diagnostics;
using Nest;

namespace IndexFile
{
    public class ElasticIndexer : IIndexer
    {
        public IndexResult IndexFiles(IndexerConfig config, IndexerCallback callback = null)
        {
            var totalWatch = new Stopwatch();
            var settings = new ConnectionSettings(new Uri(config.IndexPath)).DefaultIndex("text");
            var files = TextExtractor.GetFilesFromFolder(
                config.FolderPath,
                config.Extensions,
                config.IsRecusive);
            var client = new ElasticClient(settings);
            var result = new IndexResult
            {
                TotalFile = files.Count
            };

            ConfigMapping(client);

            totalWatch.Start();
            foreach (var file in files)
            {
                result.CurrentFile = file.FullName;
                callback?.Invoke(result);

                var stopwatch = new Stopwatch();
                var txtInfo = TextExtractor.ReadText(file.FullName);
                var request = new IndexRequest<TextInfo>
                {
                    Document = txtInfo,
                    Refresh = Elasticsearch.Net.Refresh.True
                };

                stopwatch.Start();
                client.Index(request);
                stopwatch.Stop();

                result.Time = stopwatch.Elapsed;
                result.ReadedFiles = TextExtractor.ReadedFile;
                result.Size = txtInfo.FileSize;

                callback?.Invoke(result);
            }
            totalWatch.Stop();

            result.Time = totalWatch.Elapsed;
            result.ReadedFiles = TextExtractor.ReadedFile;
            result.Size = TextExtractor.TotalSize;

            return result;
        }

        private void ConfigMapping(ElasticClient client)
        {
            client.Indices.Create("text", c => c
                .Map<TextInfo>(mappingDescriptor => mappingDescriptor
                    .Properties(propertiesDescriptor => propertiesDescriptor
                        .Text(selector => selector
                            .Name(n => n.Path)
                            .Store(true))
                         .Text(selector => selector
                            .Name(n => n.Content)
                            .Store(false)
                            .Analyzer("whitespace"))
                         .Number(selector => selector
                            .Name(n => n.FileSize)
                            .Store(true))
                         .Number(selector => selector
                            .Name(n => n.ContentLength)
                            .Store(true))
                         .Text(selector => selector
                            .Name(n => n.Name)
                            .Store(true)
                            .Analyzer("whitespace")))));
        }
    }
}