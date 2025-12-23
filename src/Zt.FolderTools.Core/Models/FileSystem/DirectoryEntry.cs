namespace Zt.FolderTools.Core.Models.FileSystem;

public class DirectoryEntry : FileSystemEntry
{
    public List<FileSystemEntry> Children { get; } = [];
}