﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            var nome = Path.GetFileName(fileName);
            if (string.IsNullOrEmpty(nome))
            {
                throw new Exception("Nome do arquivo inválido");
            }
            string filePath = Path.Combine(folderPath, nome);
            if (File.Exists(filePath))
            {
                throw new FileAlreadyExistsException(filePath);
            }
            var bytes = new Byte[fileStream.Length];
            fileStream.Read(bytes, 0, (int) fileStream.Length);
            File.WriteAllBytes(filePath, bytes);
        }

        public IEnumerable<string> ListFiles(string path)
        {
            return Directory.Exists(path) ? Directory.EnumerateFiles(path).Select(Path.GetFileName) : new List<string>().AsEnumerable();
        }

        public void Excluir(string path)
        {
            File.Delete(path);
        }
    }
}