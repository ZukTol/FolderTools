namespace Zt.FolderTools.Core.Models;

public class FileEntry : FileSystemEntry
{
    public long Size { get; set; }
    
    public DateTime LastModified { get; set; }
    
    public string? Hash { get; set; }
}