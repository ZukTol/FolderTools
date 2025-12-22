namespace Zt.FolderTools.Core.Services.Impl.HashCalculator;

public class Md5HashCalculator : IHashCalculator
{
    public string CalculateHash(string filePath)
    {
        using var stream = File.OpenRead(filePath);
        using var md5 = System.Security.Cryptography.MD5.Create();
        var hashBytes = md5.ComputeHash(stream);
        return Convert.ToHexStringLower(hashBytes);
    }

    public async Task<string> CalculateHashAsync(string filePath, CancellationToken token = default)
    {
        await using var stream = File.OpenRead(filePath);
        using var md5 = System.Security.Cryptography.MD5.Create();
        var hashBytes = await md5.ComputeHashAsync(stream, token);
        return Convert.ToHexStringLower(hashBytes);
    }
}