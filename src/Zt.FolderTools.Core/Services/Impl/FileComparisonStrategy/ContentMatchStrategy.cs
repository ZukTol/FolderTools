using Zt.FolderTools.Core.Helpers;
using Zt.FolderTools.Core.Models.Duplicates;
using Zt.FolderTools.Core.Models.FileSystem;

namespace Zt.FolderTools.Core.Services.Impl.FileComparisonStrategy;

public class ContentMatchStrategy(IHashCalculator hashCalculator) : IFileComparisonStrategy
{
    public string Name => "По содержимому (игнорируя имя)";

    public async Task<IReadOnlyList<DuplicateGroup>> FindDuplicatesAsync(
        IEnumerable<FileEntry> candidates, 
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(candidates);
        
        var byExtension = candidates.GroupBy(x => x.Extension);
        var result = new List<DuplicateGroup>();

        foreach (var group in byExtension)
        {
            var duplicates = await GroupByHashHelper.ExecuteAsync(group, hashCalculator, token);
            result.AddRange(duplicates);
        }
        return result;
    }
}