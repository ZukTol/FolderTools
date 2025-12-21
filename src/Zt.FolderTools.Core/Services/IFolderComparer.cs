using Zt.FolderTools.Core.Models;

namespace Zt.FolderTools.Core.Services;

public interface IFolderComparer
{
    IReadOnlyList<ComparisonResult> Compare(DirectoryEntry leftDir, DirectoryEntry rightDir);
}