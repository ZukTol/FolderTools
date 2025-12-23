using Zt.FolderTools.Core.Models;
using Zt.FolderTools.Core.Models.Duplicates;

namespace Zt.FolderTools.Core.Services;

public interface IDuplicateFinder
{
    Task<IReadOnlyList<DuplicateGroup>> GetDuplicatesAsync(
        IReadOnlyList<FileEntry> fileEntries,
        IFileComparisonStrategy fileComparisonStrategy,
        CancellationToken token);

    Task<IReadOnlyList<DuplicateGroup>> GetDuplicatesAsync(
        string folderPath,
        IFileComparisonStrategy fileComparisonStrategy,
        CancellationToken token);
}