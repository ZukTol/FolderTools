namespace Zt.FolderTools.Core.Services;

public interface IHashCalculator
{
    string CalculateHash(string filePath);
    
    Task<string> CalculateHashAsync(string filePath, CancellationToken token = default);
}