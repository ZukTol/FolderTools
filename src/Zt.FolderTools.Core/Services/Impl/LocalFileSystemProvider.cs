using Zt.FolderTools.Core.Models;

namespace Zt.FolderTools.Core.Services.Impl;

internal class LocalFileSystemProvider : IFileSystemProvider
{
    public DirectoryEntry GetFolderInfo(string folderPath)
    {
        ArgumentException.ThrowIfNullOrEmpty(folderPath);
        
        var rootInfo = new DirectoryInfo(folderPath);
        if (!rootInfo.Exists) 
            throw new DirectoryNotFoundException(folderPath);
        
        var result = GetFolderInfoInternal(rootInfo);
        return result;
    }

    public IReadOnlyList<FileEntry> GetFiles(string folderPath, bool recursively)
    {
        ArgumentException.ThrowIfNullOrEmpty(folderPath);
        var directoryInfo = new DirectoryInfo(folderPath);
        var result = directoryInfo.EnumerateFiles("*", 
            recursively ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)
            .Select(GetFileInfoInternal)
            .ToArray();
        return result;
    }

    private static DirectoryEntry GetFolderInfoInternal(DirectoryInfo directoryInfo)
    {
        var directoryEntry = new DirectoryEntry
        {
            FullPath = directoryInfo.FullName,
            Name = directoryInfo.Name
        };

        try
        {
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
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine($"Access denied for path: {directoryInfo.FullName}");
        }
        catch (Exception ee)
        {
            Console.WriteLine($"Exception: {ee}");
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
            Size = fileInfo.Length,
            // Hash = CalculateFileHash(fileInfo.FullName)
        };
        return fileEntry;
    }
}