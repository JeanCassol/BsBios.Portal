using System;
using System.IO;
using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.Infra.Services.Contracts;

namespace BsBios.Portal.Infra.Services.Implementations
{
    public class FileService : IFileService
    {
        public void Save(string basePath, string subFolder, string fileName, Stream fileStream)
        {
            string folderPath = Path.Combine(basePath, subFolder);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            string filePath = Path.Combine(folderPath, fileName);
            if (File.Exists(filePath))
            {
                throw new FileAlreadyExistsException(filePath);
            }
            var bytes = new Byte[fileStream.Length];
            fileStream.Read(bytes, 0, (int) fileStream.Length);
            File.WriteAllBytes(filePath, bytes);
        }

    }
}