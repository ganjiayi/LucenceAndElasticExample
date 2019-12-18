using System.Collections.Generic;
using System.IO;

namespace IndexFile
{
    public static class TextExtractor
    {
        public static long TotalSize { get; private set; }
        public static int ReadedFile { get; private set; }

        public static TextInfo ReadText(string filePath)
        {
            var txtFile = new FileInfo(filePath);
            var textInfo = new TextInfo
            {
                Path = filePath,
                FileSize = txtFile.Length,
                Name = txtFile.Name,
                Content = File.ReadAllText(filePath)
            };

            TotalSize += textInfo.FileSize;
            ReadedFile++;

            return textInfo;
        }

        public static IList<TextInfo> ReadTexts(string folderPath, string[] extensions, bool recusive = true)
        {
            var txtInfos = new List<TextInfo>();
            var files = GetFilesFromFolder(folderPath, extensions, recusive);

            foreach (var file in files)
            {
                var txtInfo = ReadText(file.FullName);

                txtInfos.Add(txtInfo);
            }

            return txtInfos;
        }

        private static IList<FileInfo> GetFilesFromFolderHelper(string folderPath, string[] extensions)
        {
            var directory = new DirectoryInfo(folderPath);
            var files = new List<FileInfo>();

            foreach (var extension in extensions)
            {
                files.AddRange(directory.GetFiles($"*.{extension}"));
            }

            return files;
        }

        public static IList<FileInfo> GetFilesFromFolder(string folderPath, string[] extensions, bool recusive = true)
        {
            var txtFiles = new List<FileInfo>();
            if (recusive)
            {
                txtFiles.AddRange(GetFilesFromFolderHelper(folderPath, extensions));
                var directory = new DirectoryInfo(folderPath);
                var subDirectories = directory.GetDirectories();

                foreach (var subDirectory in subDirectories)
                {
                    txtFiles.AddRange(GetFilesFromFolder(subDirectory.FullName, extensions, recusive));
                }
            }
            else
            {
                txtFiles.AddRange(GetFilesFromFolderHelper(folderPath, extensions));
            }

            return txtFiles;
        }
    }
}