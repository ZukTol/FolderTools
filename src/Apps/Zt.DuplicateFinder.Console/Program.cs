using ConsoleAppFramework;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Zt.FolderTools.Core.Services;
using Zt.FolderTools.Core.Services.Impl;
using Zt.FolderTools.Core.Services.Impl.FileComparisonStrategy;
using Zt.FolderTools.Core.Services.Impl.HashCalculator;

var cts = new CancellationTokenSource();

var app = ConsoleApp.Create();
app.ConfigureServices(services =>
{
    services.AddSingleton<IFileSystemProvider, LocalFileSystemProvider>();
    services.AddSingleton<IDuplicateFinder, DuplicateFinder>();
    services.AddSingleton<IHashCalculator, Sha256HashCalculator>();
    services.AddKeyedSingleton<IFileComparisonStrategy, StrictMatchStrategy>("strict");
    services.AddKeyedSingleton<IFileComparisonStrategy, ContentMatchStrategy>("content");
});

app.Add<Commands>();
await app.RunAsync(args, cts.Token);

internal class Commands
{
    /// <summary>
    /// Find duplicates in folder recursively.
    /// </summary>
    /// <param name="path">-p, Folder path to analyze.</param>
    /// <param name="mode">-m, Comparison mode: content, strict</param>
    /// <param name="outputFile">-o, Path of file with results</param>
    [Command("")]
    public async Task FindDuplicatesAsync(
        [FromServices]IServiceProvider serviceProvider, 
        string path, 
        string mode,
        string outputFile,
        CancellationToken cancellationToken)
    {
        Console.WriteLine($"Will find duplicates in {path}, mode: {mode}");
        var duplicateFinder = serviceProvider.GetRequiredService<IDuplicateFinder>();
        var strategy = serviceProvider.GetKeyedService<IFileComparisonStrategy>(mode)
            ?? serviceProvider.GetKeyedService<IFileComparisonStrategy>("strict");
        var duplicates = await duplicateFinder.GetDuplicatesAsync(path, strategy, cancellationToken);

        if(!string.IsNullOrEmpty(outputFile))
            File.Delete(outputFile);
        
        foreach (var duplicateGroup in duplicates)
        {
            var groupName = $"{duplicateGroup.GroupName[..10]}, Size: {ConvertSize(duplicateGroup.Files[0].Size)}";
            if (!string.IsNullOrEmpty(outputFile))
            {
                await File.AppendAllLinesAsync(outputFile, [groupName], cancellationToken);
                await File.AppendAllLinesAsync(outputFile, duplicateGroup.Files.Select(f => $"\t{f.FullPath}"), cancellationToken);
            }
            
            Console.WriteLine(groupName);
            foreach (var file in duplicateGroup.Files)
            {
                Console.WriteLine($"\t{file.FullPath}");
            }
        }
    }

    private string ConvertSize(long size)
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