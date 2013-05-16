using System;
using System.IO;

namespace BsBios.Portal.Common.Exceptions
{
    public class FileAlreadyExistsException : Exception
    {
        private readonly string _filePath;
        public FileAlreadyExistsException(string filePath)
        {
            _filePath = filePath;
        }

        public override string Message
        {
            get { return "O arquivo " + Path.GetFileName(_filePath)  + " já existe."; }
        }
    }
}