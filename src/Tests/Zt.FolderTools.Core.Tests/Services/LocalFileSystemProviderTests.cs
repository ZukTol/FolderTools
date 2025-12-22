using Zt.FolderTools.Core.Services.Impl;

namespace Zt.FolderTools.Core.Tests.Services;

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

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void GetFiles_ThrowsException_WhenPathIsInvalid(bool recursively)
    {
        var sut = new LocalFileSystemProvider();
        Assert.Throws<ArgumentException>(() => sut.GetFiles(string.Empty, recursively));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public Task GetFiles_ReturnsFiles_WhenEverythingOk(bool recursively)
    {
        var sut = new LocalFileSystemProvider();
        var result = sut.GetFiles("TestFiles", recursively);
        Assert.NotNull(result);
        return Verify(result)
            .UseParameters(recursively);
    }
}