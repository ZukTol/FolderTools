using Zt.FolderTools.Core.Models.FileSystem;
using Zt.FolderTools.Core.Models.FolderSync;

namespace Zt.FolderTools.Core.Services;

public interface IFolderComparer
{
    IReadOnlyList<ComparisonResult> Compare(DirectoryEntry leftDir, DirectoryEntry rightDir);
}