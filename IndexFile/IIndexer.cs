namespace IndexFile
{
    public interface IIndexer
    {
        IndexResult IndexFiles(IndexerConfig config, IndexerCallback callback = null);
    }
}