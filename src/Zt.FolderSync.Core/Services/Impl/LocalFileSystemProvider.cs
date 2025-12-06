using Zt.FolderSync.Core.Models;

namespace Zt.FolderSync.Core.Services.Impl;

public class LocalFileSystemProvider : IFileSystemProvider
{
    public DirectoryEntry GetDirectoryInfo(string path)
    {
        var directoryInfo = new System.IO.DirectoryInfo(path);
        throw new NotImplementedException();
    }
}