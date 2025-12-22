using Zt.FolderTools.Core.Models;
using Zt.FolderTools.Core.Models.Duplicates;

namespace Zt.FolderTools.Core.Services.Impl;

public class DuplicateFinder(IFileSystemProvider fileSystemProvider) : IDuplicateFinder
{
    public IReadOnlyList<DuplicateGroup> GetDuplicates(IReadOnlyList<FileEntry> fileEntries)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyList<DuplicateGroup> GetDuplicates(string folderPath)
    {
        var files = fileSystemProvider.GetFiles(folderPath, recursively: true);
        
        throw new NotImplementedException();
    }
}