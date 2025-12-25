using Moq;
using Zt.FolderTools.Core.Models.FileSystem;
using Zt.FolderTools.Core.Services;
using Zt.FolderTools.Core.Services.Impl.FileComparisonStrategy;

namespace Zt.FolderTools.Core.Tests.Services.FileComparisonStrategy;

public class StrictMatchStrategyTests
{
    private readonly StrictMatchStrategy _strategy;
    private readonly Mock<IHashCalculator> _hasherMock = new Mock<IHashCalculator>();
    private readonly List<string> _tempFiles;
    
    private void SetupHashCalculator(IReadOnlyList<FileEntry> fileEntries)
    {
        foreach (var fileEntry in fileEntries)
        {
            _hasherMock.Setup(h => h.CalculateHash(fileEntry.FullPath))
                .Returns(fileEntry.Size.ToString());
            _hasherMock.Setup(h => h.CalculateHashAsync(fileEntry.FullPath))
                .ReturnsAsync(fileEntry.Size.ToString());
        }
    }
}