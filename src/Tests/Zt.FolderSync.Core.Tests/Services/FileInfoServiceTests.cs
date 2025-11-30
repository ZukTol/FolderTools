using Zt.FolderSync.Core.Services.Impl;

namespace Zt.FolderSync.Core.Tests.Services;

public class FileInfoServiceTests
{
    [Fact]
    public void GetFileInfo_Throws_WhenEmptyPath()
    {
        var sut = new FileInfoService();
        
        Assert.Throws<ArgumentException>(() => sut.GetInfo(string.Empty));
    }
}