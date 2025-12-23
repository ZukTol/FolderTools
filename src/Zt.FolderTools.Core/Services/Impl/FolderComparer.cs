using Zt.FolderTools.Core.Models;
using Zt.FolderTools.Core.Models.FileSystem;
using Zt.FolderTools.Core.Models.FolderSync;

namespace Zt.FolderTools.Core.Services.Impl;

public class FolderComparer : IFolderComparer
{
    public IReadOnlyList<ComparisonResult> Compare(DirectoryEntry leftDir, DirectoryEntry rightDir)
    {
        return CompareDirectories(leftDir, rightDir, string.Empty);
    }
    
    private static IReadOnlyList<ComparisonResult> CompareDirectories(DirectoryEntry leftDir, DirectoryEntry rightDir, string relativePath)
    {
        var result = new List<ComparisonResult>();
        
        var leftMap = leftDir.Children.ToDictionary(x => x.Name);
        var rightMap = rightDir.Children.ToDictionary(x=>x.Name);

        var allKeys = leftMap.Keys.Union(rightMap.Keys);

        foreach (var entryName in allKeys)
        {
            leftMap.TryGetValue(entryName,  out var leftEntry);
            rightMap.TryGetValue(entryName, out var rightEntry);
            
            var currentRelativePath = Path.Combine(relativePath, entryName);

            if (leftEntry is null)
            {
                result.Add(new ComparisonResult(EntryStatus.RightOnly, null, rightEntry, currentRelativePath));
                continue;
            }
            if (rightEntry is null)
            {
                result.Add(new ComparisonResult(EntryStatus.LeftOnly, leftEntry, null, currentRelativePath));
                continue;
            }
            if (leftEntry.GetType() != rightEntry.GetType())
            {
                result.Add(new ComparisonResult(EntryStatus.TypeMismatch, leftEntry, rightEntry, currentRelativePath));
                continue;
            }
            if (leftEntry is DirectoryEntry leftSubDir)
            {
                result.AddRange(CompareDirectories(leftSubDir, (DirectoryEntry)rightEntry, currentRelativePath));
            }
            
            else if (leftEntry is FileEntry leftFile)
            {
                var rightFile = (FileEntry)rightEntry;
                result.Add(leftFile.Hash != rightFile.Hash
                    ? new ComparisonResult(EntryStatus.Different, leftFile, rightFile, currentRelativePath)
                    // Опционально: можно не добавлять Identical, чтобы уменьшить объем данных
                    : new ComparisonResult(EntryStatus.Identical, leftFile, rightFile, currentRelativePath));
            }
        }

        return result;
    }
}