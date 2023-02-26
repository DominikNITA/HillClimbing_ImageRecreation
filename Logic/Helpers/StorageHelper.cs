using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Helpers
{
    public class StorageHelper
    {
        const string GlobalFolderName = "algorithm-data";
        const string UnsafeUploadsName = "unsafe_uploads";
        const string GeneratedImagesFolderName = "iterations";
        const string GeneratedVideoFileNamePrefix = "timelapse-";

        public static readonly string GlobalFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", GlobalFolderName);

        public static readonly string UnsafeUploadsPath = Path.Combine(
           GlobalFolderPath,
           UnsafeUploadsName);


        static public string GetPathInFolder(string folderPath, string fileName)
        {
            return Path.Combine(folderPath, fileName);
        }

        static public string GetPathForFileInUnsafeUploads(string fileName)
        {
            return GetPathInFolder(UnsafeUploadsPath, fileName);
        }

        static public string CreateRandomFileNameWithExtension(string originalFileName)
        {
            var randomFileName = Path.GetRandomFileName();
            return string.Concat(
                randomFileName.AsSpan(0, randomFileName.LastIndexOf('.')), //random file name
                originalFileName.AsSpan(originalFileName.LastIndexOf('.'))); // original extension
        }

        static public string GetPathForFolderById(string id)
        {
            return Path.Combine(GlobalFolderPath, id);
        }

        static public string GetPathForIterationsFolderById(string id)
        {
            return Path.Combine(GetPathForFolderById(id), GeneratedImagesFolderName);
        }

        static public string GetPathForIterationImage(string id, int iteration, string extension = "png")
        {
            return Path.Combine(
                GetPathForIterationsFolderById(id),
                $"{iteration.ToString("00000")}.{extension}"
                );
        }

        static public string GetPathForTimelapseFile(string id, string extension = "mp4")
        {
            return Path.Combine(
                GetPathForFolderById(id),
                $"{GeneratedVideoFileNamePrefix}{id}.{extension}"
                );
        }

        static public string ConvertPathToRelativeToWwwroot(string path)
        {
            return path.Substring(path.LastIndexOf(GlobalFolderName));
        }
    }
}
