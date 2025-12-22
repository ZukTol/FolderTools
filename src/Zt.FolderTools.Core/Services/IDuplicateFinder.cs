using Zt.FolderTools.Core.Models;
using Zt.FolderTools.Core.Models.Duplicates;

namespace Zt.FolderTools.Core.Services;

public interface IDuplicateFinder
{
    IReadOnlyList<DuplicateGroup> GetDuplicates(IReadOnlyList<FileEntry> fileEntries);
    
    IReadOnlyList<DuplicateGroup> GetDuplicates(string folderPath);
}