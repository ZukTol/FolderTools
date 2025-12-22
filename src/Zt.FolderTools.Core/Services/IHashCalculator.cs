namespace Zt.FolderTools.Core.Services;

public interface IHashCalculator
{
    string CalculateHash(string filePath);
}