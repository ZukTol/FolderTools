using Zt.FolderSync.Core.Models;

namespace Zt.FolderSync.Core.Services;

public interface IFileSystemProvider
{
    DirectoryEntry GetFolderInfo(string path);
}