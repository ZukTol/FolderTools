using Zt.FolderTools.Core.Models;

namespace Zt.FolderTools.Core.Services;

public interface IFileSystemProvider
{
    DirectoryEntry GetFolderInfo(string path);
}