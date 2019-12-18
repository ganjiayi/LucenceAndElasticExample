using CommandLine;

namespace IndexFile
{
    public class IndexerConfig
    {
        [Option('l', "UseLucene", HelpText = "Use lucence or elasticsearch", Default = false)]
        public bool UseLucene { get; set; }

        [Option('f', "FolderPath", Required = true, HelpText = "Path to folder where you want to index")]
        public string FolderPath { get; set; }

        [Option('r', "Recusive", Default = false, HelpText = "Find files in subfolder")]
        public bool IsRecusive { get; set; }

        [Option('e', "Extensions", Separator = ',', Default = new string[] { "txt" }, HelpText = "File extension separated by a comma ','")]
        public string[] Extensions { get; set; }

        [Option('i', "IndexPath", Required = true, HelpText = "Index Folder for Lucene and Url for Elastic")]
        public string IndexPath { get; set; }

        [Option('x', "Exclude",
            Separator = ',',
            Default = new string[] { "FileSize", "ContentLength", "Name" },
            HelpText = "Exclude Properties. Properties: Path, Content, FileSize, ContentLength, Name")]
        public string[] Excludes { get; set; }

        public override string ToString()
        {
            return $"Folder: {FolderPath}\nRecusive: {IsRecusive}\nExtensions: {string.Join(", ", Extensions)}\nIndex Path: {IndexPath}";
        }
    }
}