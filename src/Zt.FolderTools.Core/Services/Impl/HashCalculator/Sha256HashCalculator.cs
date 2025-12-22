namespace Zt.FolderTools.Core.Services.Impl.HashCalculator;

public class Sha256HashCalculator : IHashCalculator
{
    public string CalculateHash(string filePath)
    {
        using var stream = File.OpenRead(filePath);
        using var sha = System.Security.Cryptography.SHA256.Create();
        var hash = sha.ComputeHash(stream);
        return Convert.ToHexStringLower(hash);
    }

    public async Task<string> CalculateHashAsync(string filePath, CancellationToken token = default)
    {
        await using var stream = File.OpenRead(filePath);
        using var sha = System.Security.Cryptography.SHA256.Create();
        var hash = await sha.ComputeHashAsync(stream, token);
        return Convert.ToHexStringLower(hash);
    }
}