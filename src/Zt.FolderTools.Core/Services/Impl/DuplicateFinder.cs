using Zt.FolderTools.Core.Models;
using Zt.FolderTools.Core.Models.Duplicates;

namespace Zt.FolderTools.Core.Services.Impl;

public class DuplicateFinder(IFileSystemProvider fileSystemProvider) : IDuplicateFinder
{
    public Task<IReadOnlyList<DuplicateGroup>> GetDuplicatesAsync(
        string folderPath, 
        IFileComparisonStrategy fileComparisonStrategy,
        CancellationToken token)
    {
        ArgumentException.ThrowIfNullOrEmpty(folderPath);
        ArgumentNullException.ThrowIfNull(fileComparisonStrategy);
        
        var files = fileSystemProvider.GetFiles(folderPath, recursively: true);
        
        return GetDuplicatesAsync(files, fileComparisonStrategy, token);
    }
    
    public async Task<IReadOnlyList<DuplicateGroup>> GetDuplicatesAsync(
        IReadOnlyList<FileEntry> fileEntries,
        IFileComparisonStrategy fileComparisonStrategy,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(fileEntries);
        ArgumentNullException.ThrowIfNull(fileComparisonStrategy);
        
        var potentialDuplicates = fileEntries
            .GroupBy(f => f.Size)
            .Where(g => g.Count() > 1)
            .ToArray();

        var finalResults = new List<DuplicateGroup>(potentialDuplicates.Length);
        
        foreach (var sizeGroup in potentialDuplicates)
        {
            token.ThrowIfCancellationRequested();

            var duplicates = await fileComparisonStrategy.FindDuplicatesAsync(sizeGroup, token);
            finalResults.AddRange(duplicates);
        }
        return finalResults;
    }
}