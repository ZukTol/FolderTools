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
    [Command("")]
    public async Task FindDuplicatesAsync([FromServices]IServiceProvider serviceProvider, string path, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Will find duplicates in {path}");
        var duplicateFinder = serviceProvider.GetRequiredService<IDuplicateFinder>();
        var strategy = serviceProvider.GetRequiredKeyedService<IFileComparisonStrategy>("strict");
        var duplicates = await duplicateFinder.GetDuplicatesAsync(path, strategy, cancellationToken);

        foreach (var duplicateGroup in duplicates)
        {
            Console.WriteLine(duplicateGroup.GroupName);
            foreach (var file in duplicateGroup.Files)
            {
                Console.WriteLine($"\t{file.FullPath}");
            }
        }
    }
}