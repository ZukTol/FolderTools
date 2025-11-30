namespace Zt.FolderSync.Core.Services.Impl;

public class FileInfoService : IFileInfoService
{
    public FileInfo GetInfo(string filePath)
    {
        ArgumentException.ThrowIfNullOrEmpty(filePath);

        throw new NotImplementedException();
    }
}