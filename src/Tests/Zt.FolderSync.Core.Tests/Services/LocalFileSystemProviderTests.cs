using Zt.FolderSync.Core.Services.Impl;

namespace Zt.FolderSync.Core.Tests.Services;

public class LocalFileSystemProviderTests
{
    [Fact]
    public void GetFolderInfo_ThrowsException_WhenPathIsInvalid()
    {
        var sut = new LocalFileSystemProvider();
        Assert.Throws<ArgumentException>(() => sut.GetFolderInfo(string.Empty));
    }

    [Fact]
    public Task GetFolderInfo_ReturnsFolderStructure_WhenEverythingOk()
    {
        var sut = new LocalFileSystemProvider();
        var folder = sut.GetFolderInfo("TestFiles");
        Assert.NotNull(folder);
        return Verify(folder);
    }
}