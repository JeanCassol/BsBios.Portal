using System.IO;

namespace BsBios.Portal.Infra.Services.Contracts
{
    public interface IFileService
    {
        void Save(string basePath, string subFolder, string fileName, Stream fileStream);
    }
}