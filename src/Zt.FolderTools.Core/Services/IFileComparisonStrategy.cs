using Zt.FolderTools.Core.Models.Duplicates;
using Zt.FolderTools.Core.Models.FileSystem;

namespace Zt.FolderTools.Core.Services;

public interface IFileComparisonStrategy
{
    string Name { get; }
    
    Task<IEnumerable<DuplicateGroup>> FindDuplicatesAsync(
        IEnumerable<FileEntry> candidates, 
        CancellationToken token);
}