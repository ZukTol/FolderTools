using Zt.FolderTools.Core.Models;
using Zt.FolderTools.Core.Models.Duplicates;

namespace Zt.FolderTools.Core.Services;

public interface IFileComparisonStrategy
{
    string Name { get; }
    
    Task<IEnumerable<DuplicateGroup>> FindDuplicatesAsync(
        IEnumerable<FileEntry> candidates, 
        CancellationToken token);
}