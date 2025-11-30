namespace Zt.FolderSync.Core.Services;

public interface IFileInfoService
{
    public FileInfo GetInfo(string filePath);
}