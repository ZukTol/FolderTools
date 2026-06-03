using ConsoleAppFramework;
using Microsoft.Extensions.DependencyInjection;
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

await app.RunAsync(args, cts.Token);
