using Zt.FolderTools.Core.Models.FileSystem;

namespace Zt.FolderTools.Core.Services;

public interface IFileSystemProvider
{
    DirectoryEntry GetFolderInfo(string folderPath);
    
    IReadOnlyList<FileEntry> GetFiles(string folderPath, bool recursively);
}