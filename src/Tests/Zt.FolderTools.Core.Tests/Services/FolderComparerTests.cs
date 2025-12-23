using Zt.FolderTools.Core.Services.Impl;

namespace Zt.FolderTools.Core.Tests.Services;

public class FolderComparerTests
{
    [Fact]
    public Task Compare_ReturnsResults_WhenEverythingOk()
    {
        var sut = new FolderComparer();
        var left = new LocalFileSystemProvider().GetFolderInfo("TestFiles");
        var right = new LocalFileSystemProvider().GetFolderInfo("TestFilesSecond");

        var result = sut.Compare(left, right);

        Assert.NotNull(result);
        return Verify(result);
    }
}
