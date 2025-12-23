using Zt.FolderTools.Core.Models.FileSystem;

namespace Zt.FolderTools.Core.Models.Duplicates;

public class DuplicateGroup(string groupName)
{
    public string GroupName { get; set; } = groupName;

    public IReadOnlyList<FileEntry> Files { get; set; } = [];
}