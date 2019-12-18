using System;

namespace IndexFile
{
    public class IndexResult
    {
        public int TotalFile { get; set; }

        public TimeSpan Time { get; set; }

        public long Size { get; set; }

        public int ReadedFiles { get; set; }

        public string CurrentFile { get; set; }

        public override string ToString()
        {
            return $"Path: {CurrentFile}\nSize: {(Size / 1024).ToString("#,#")} KB\nTime: {Time}";
        }
    }
}