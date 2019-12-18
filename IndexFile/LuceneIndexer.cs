using System;
using System.Collections.Generic;
using System.Diagnostics;
using Lucene.Net.Analysis;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Store;

namespace IndexFile
{
    public class LuceneIndexer : IIndexer
    {
        public IndexResult IndexFiles(IndexerConfig config, IndexerCallback callback = null)
        {
            var totalWatch = new Stopwatch();
            var directory = FSDirectory.Open(config.IndexPath);
            var analyzer = new WhitespaceAnalyzer();
            var files = TextExtractor.GetFilesFromFolder(
                config.FolderPath,
                config.Extensions,
                config.IsRecusive);
            var result = new IndexResult
            {
                TotalFile = files.Count
            };

            totalWatch.Start();
            foreach (var file in files)
            {
                result.CurrentFile = file.FullName;
                callback?.Invoke(result);

                using (var writer = new IndexWriter(directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    var stopwatch = new Stopwatch();
                    var document = new Document();
                    var txtInfo = TextExtractor.ReadText(file.FullName);
                    var fields = GetFields(txtInfo, config.Excludes);

                    foreach (var field in fields)
                    {
                        document.Add(field);
                    }

                    stopwatch.Start();

                    writer.AddDocument(document, analyzer);
                    writer.Optimize();

                    stopwatch.Stop();

                    result.Time = stopwatch.Elapsed;
                    result.ReadedFiles = TextExtractor.ReadedFile;
                    result.Size = txtInfo.FileSize;

                    callback?.Invoke(result);
                }
            }
            totalWatch.Stop();

            result.Time = totalWatch.Elapsed;
            result.ReadedFiles = TextExtractor.ReadedFile;
            result.Size = TextExtractor.TotalSize;

            return result;
        }

        private static IList<IFieldable> GetFields(TextInfo textInfo, string[] excludes)
        {
            var fields = new List<IFieldable>();

            if (Array.IndexOf(excludes, nameof(textInfo.Path)) > -1)
            {
                var pathField = new Field(nameof(textInfo.Path), textInfo.Path, Field.Store.YES, Field.Index.NO);
                fields.Add(pathField);
            }

            if (Array.IndexOf(excludes, nameof(textInfo.Name)) > -1)
            {
                var nameField = new Field(nameof(textInfo.Name), textInfo.Name, Field.Store.YES, Field.Index.ANALYZED);
                fields.Add(nameField);
            }

            if (Array.IndexOf(excludes, nameof(textInfo.FileSize)) > -1)
            {
                var fileSizeField = new NumericField(nameof(textInfo.FileSize), Field.Store.YES, true);
                fileSizeField.SetLongValue(textInfo.FileSize);
                fields.Add(fileSizeField);
            }

            if (Array.IndexOf(excludes, nameof(textInfo.FileSize)) > -1)
            {
                var contentLengthField = new NumericField(nameof(textInfo.ContentLength), Field.Store.YES, true);
                contentLengthField.SetLongValue(textInfo.ContentLength);
                fields.Add(contentLengthField);
            }

            if (Array.IndexOf(excludes, nameof(textInfo.FileSize)) > -1)
            {
                var contentField = new Field(nameof(textInfo.Content), textInfo.Content, Field.Store.NO, Field.Index.ANALYZED);
                fields.Add(contentField);
            }

            return fields;
        }
    }
}