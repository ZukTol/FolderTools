using ConsoleAppFramework;
using Microsoft.Extensions.DependencyInjection;
using Zt.FolderTools.Core.Services;

namespace Zt.FolderTools.Commands;

[RegisterCommands]
internal class FindDuplicatesCommand
{
    /// <summary>
    /// Find duplicates in folder recursively.
    /// </summary>
    /// <param name="path">-p, Folder path to analyze.</param>
    /// <param name="mode">-m, Comparison mode: content, strict</param>
    /// <param name="outputFile">-o, Path of file with results</param>
    public async Task FindDuplicatesAsync(
        [FromServices] IServiceProvider serviceProvider,
        string path,
        string mode,
        string outputFile,
        CancellationToken cancellationToken)
    {
        System.Console.WriteLine($"Will find duplicates in {path}, mode: {mode}");
        var duplicateFinder = serviceProvider.GetRequiredService<IDuplicateFinder>();
        var strategy = serviceProvider.GetKeyedService<IFileComparisonStrategy>(mode)
            ?? serviceProvider.GetRequiredKeyedService<IFileComparisonStrategy>("strict");
        var duplicates = await duplicateFinder.GetDuplicatesAsync(path, strategy, cancellationToken);

        if (!string.IsNullOrEmpty(outputFile))
            File.Delete(outputFile);

        foreach (var duplicateGroup in duplicates.OrderByDescending(x => x.Files[0].Size))
        {
            var groupName = $"{duplicateGroup.GroupName[..10]}, Size: {ConvertSize(duplicateGroup.Files[0].Size)}";
            if (!string.IsNullOrEmpty(outputFile))
            {
                await File.AppendAllLinesAsync(outputFile, [groupName], cancellationToken);
                await File.AppendAllLinesAsync(outputFile, duplicateGroup.Files.Select(f => $"\t{f.FullPath}"), cancellationToken);
            }

            System.Console.WriteLine(groupName);
            foreach (var file in duplicateGroup.Files)
            {
                System.Console.WriteLine($"\t{file.FullPath}");
            }
        }
    }

    private static string ConvertSize(long size)
    {
        string[] units = ["B", "kB", "MB", "GB", "TB"];

        double result = size;
        var i = 0;

        do
        {
            result /= 1024.0;
            i++;
        } while (result > 1024);

        return $"{Math.Round(result, 2)} {units[i]}";
    }
}
