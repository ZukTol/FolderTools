using Zt.FolderTools.Core.Models.FileSystem;

namespace Zt.FolderTools.Core.Models.FolderSync;

public class ComparisonResult(EntryStatus status, FileSystemEntry? leftEntry, FileSystemEntry? rightEntry, string relativePath)
{
    public EntryStatus Status { get; } = status;
    
    public FileSystemEntry? LeftEntry { get; } = leftEntry;
    
    public FileSystemEntry? RightEntry { get; } = rightEntry;
    
    public string RelativePath { get; } =  relativePath;
}