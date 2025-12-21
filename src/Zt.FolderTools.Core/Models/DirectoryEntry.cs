namespace Zt.FolderTools.Core.Models;

public class DirectoryEntry : FileSystemEntry
{
    public List<FileSystemEntry> Children { get; } = [];
}