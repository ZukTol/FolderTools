# AGENTS.md

## Project overview

A .NET 10 tool for finding duplicate files and comparing folders. Two main features: duplicate detection (`find-duplicates`) and folder comparison (core library).

## Solution structure

- `Zt.FolderTools.slnx` (new .slnx format, not .sln)
- `src/Zt.FolderTools.Core/` — library: models, service interfaces, implementations
- `src/Zt.FolderTools.Console/` — CLI app using ConsoleAppFramework v5 (source-generator-based command registration via `[RegisterCommands]`)
- `src/Tests/Zt.FolderTools.Core.Tests/` — xUnit + Verify snapshot tests + Moq

## Build & test

```bash
dotnet build Zt.FolderTools.slnx
dotnet test Zt.FolderTools.slnx
```

Run a single test project:

```bash
dotnet test src/Tests/Zt.FolderTools.Core.Tests/Zt.FolderTools.Core.Tests.csproj
```

## Key conventions

- **ConsoleAppFramework v5**: Commands are auto-registered via `[RegisterCommands]` + source generator. Do not manually wire commands in `Program.cs`; add a new command class instead.
- **Keyed DI**: `IFileComparisonStrategy` uses `AddKeyedSingleton` with string keys (`"strict"`, `"content"`). New strategies must be registered with a key.
- **InternalsVisibleTo**: Core exposes internals to both `.Core.Tests` and `.Console` via `AssemblyInfo.cs`.
- **Primary constructors**: Used for DI (e.g. `DuplicateFinder(IFileSystemProvider fileSystemProvider)`).

## Testing notes

- **Verify (snapshot testing)**: Several tests use `Verify.Xunit` for snapshot assertions. If snapshots need updating, run with `--verify` or set env var `Verify_AutoVerify=true`, then review diffs.
- **Test fixtures**: Tests reference `TestFiles/` and `TestFilesSecond/` directories under the test project. These are copied to output via `CopyToOutputDirectory`.
- Verified `.txt` files live alongside test classes and are declared as `DependentUpon` the test `.cs` file.
