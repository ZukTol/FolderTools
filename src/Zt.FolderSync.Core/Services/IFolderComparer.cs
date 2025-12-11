using Zt.FolderSync.Core.Models;

namespace Zt.FolderSync.Core.Services;

public interface IFolderComparer
{
    IReadOnlyList<ComparisonResult> Compare(DirectoryEntry leftDir, DirectoryEntry rightDir);
}