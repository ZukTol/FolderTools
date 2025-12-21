using System.Security.Cryptography;
using Zt.FolderTools.Core.Models;
using Zt.FolderTools.Core.Services;

namespace Zt.FolderSync.Core.Services.Impl;

internal class LocalFileSystemProvider : IFileSystemProvider
{
    public DirectoryEntry GetFolderInfo(string path)
    {
        ArgumentException.ThrowIfNullOrEmpty(path);
        
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
            Hash = CalculateFileHash(fileInfo.FullName)
        };
        return fileEntry;
    }
    
    private static string CalculateFileHash(string filePath)
    {
        using var sha256 = SHA256.Create();
        using var stream = File.OpenRead(filePath);
        var hashBytes = sha256.ComputeHash(stream);
        return Convert.ToHexStringLower(hashBytes);
    }
}