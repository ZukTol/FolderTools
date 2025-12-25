namespace Zt.FolderTools.Core.Models.FileSystem;

public class FileEntry : FileSystemEntry
{
    public long Size { get; set; }
    
    public DateTime LastModified { get; set; }
    
    public string Extension { get; set; }
    
    public string? Hash { get; set; }
}