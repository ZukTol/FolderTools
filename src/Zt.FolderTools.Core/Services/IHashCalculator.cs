namespace Zt.FolderTools.Core.Services;

public interface IHashCalculator
{
    string CalculateHash(string filePathult);
    
    Task<string> CalculateHashAsync(string filePath, CancellationToken token = default);
}