namespace IndexFile
{
    public class TextInfo
    {
        public string Path { get; set; }

        public string Content { get; set; }

        public long FileSize { get; set; }

        public long ContentLength => string.IsNullOrEmpty(Content) ? 0 : Content.Length;

        public string Name { get; set; }

        public override string ToString()
        {
            return $"Name: {Name}\nPath: {Path}\nFile size: {FileSize}\nContent Length: {ContentLength}\n";
        }
    }
}