using Zt.FolderSync.Core.Models;

namespace Zt.FolderSync.Core.Services.Impl;

internal class LocalFileSystemProvider : IFileSystemProvider
{
    public DirectoryEntry GetFolderInfo(string path)
    {
        var rootInfo = new DirectoryInfo(path);
        if (!rootInfo.Exists) 
            throw new DirectoryNotFoundException(path);
        
        var result = GetFolderInfoInternal(rootInfo);
        return result;
    }

    private static DirectoryEntry GetFolderInfoInternal(DirectoryInfo directoryInfo)
    {
        var directoryEntry = new DirectoryEntry
        {
            FullPath = directoryInfo.FullName,
            Name = directoryInfo.Name,
            Children = []
        };

        foreach (var fileInfo in directoryInfo.EnumerateFiles())
        {
            var fileEntry = GetFileInfoInternal(fileInfo);
            directoryEntry.Children.Add(fileEntry);
        }
        
        foreach (var subDirInfo in directoryInfo.EnumerateDirectories())
        {
            if ((subDirInfo.Attributes & FileAttributes.System) == 0)
            {
                directoryEntry.Children.Add(GetFolderInfoInternal(subDirInfo));
            }
        }
        
        return directoryEntry;
    }

    private static FileEntry GetFileInfoInternal(FileInfo fileInfo)
    {
        var fileEntry = new FileEntry
        {
            FullPath = fileInfo.FullName,
            Name = fileInfo.Name,
            LastModified = fileInfo.LastWriteTimeUtc,
            Size = fileInfo.Length
        };
        return fileEntry;
    }
}