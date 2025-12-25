using Zt.FolderTools.Core.Helpers;
using Zt.FolderTools.Core.Models.Duplicates;
using Zt.FolderTools.Core.Models.FileSystem;

namespace Zt.FolderTools.Core.Services.Impl.FileComparisonStrategy;

public class StrictMatchStrategy(IHashCalculator hashCalculator) : IFileComparisonStrategy
{
    public string Name => "Strict match";
    
    public async Task<IReadOnlyList<DuplicateGroup>> FindDuplicatesAsync(
        IEnumerable<FileEntry> candidates,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(candidates);
        
        var result = new List<DuplicateGroup>();

        // Группируем сначала по имени и расширению (быстрая операция)
        var byName = candidates.GroupBy(x => new { x.Name, x.Extension });

        foreach (var group in byName.Where(g => g.Count() > 1))
        {
            // Внутри групп с одинаковым именем проверяем хэш
            var byHash = await GroupByHashHelper.ExecuteAsync(group, hashCalculator, token);
            result.AddRange(byHash);
        }

        return result;
    }
}