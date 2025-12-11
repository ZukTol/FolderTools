namespace Zt.FolderSync.Core.Models;

public class DirectoryEntry : FileSystemEntry
{
    public List<FileSystemEntry> Children { get; } = [];
}