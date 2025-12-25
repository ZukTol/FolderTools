using System.Collections.Concurrent;
using Zt.FolderTools.Core.Models.Duplicates;
using Zt.FolderTools.Core.Models.FileSystem;
using Zt.FolderTools.Core.Services;

namespace Zt.FolderTools.Core.Helpers;

public static class GroupByHashHelper
{
    // Ограничиваем количество одновременных операций чтения с диска.
    // Для HDD оптимально 1-2, для SSD можно больше (4-8).
    // Это число можно вынести в конфигурацию.
    private const int MaxDegreeOfParallelism = 4;

    /// <summary>
    /// Вычисляет хэши для списка файлов и группирует их.
    /// </summary>
    public static async Task<IEnumerable<DuplicateGroup>> ExecuteAsync(
        IEnumerable<FileEntry> files,
        IHashCalculator hasher,
        CancellationToken token)
    {
        var filesList = files.ToList();

        // Если файл всего один или список пуст, дубликатов быть не может.
        if (filesList.Count < 2)
        {
            return Enumerable.Empty<DuplicateGroup>();
        }

        // Используем ConcurrentBag для потокобезопасного хранения результатов
        var hashedFiles = new ConcurrentBag<(FileEntry File, string Hash)>();

        // Семафор для ограничения одновременного доступа к диску
        using var semaphore = new SemaphoreSlim(MaxDegreeOfParallelism);

        var tasks = filesList.Select(async file =>
        {
            // Ожидаем свободный слот
            await semaphore.WaitAsync(token);

            try
            {
                // Двойная проверка отмены перед тяжелой операцией
                if (token.IsCancellationRequested) return;

                var hash = await hasher.CalculateHashAsync(file.FullPath, token);
                hashedFiles.Add((file, hash));
            }
            finally
            {
                // Всегда освобождаем слот, даже при ошибке
                semaphore.Release();
            }
        });

        // Запускаем и ждем завершения всех задач
        await Task.WhenAll(tasks);

        // Группировка результатов (уже в памяти, CPU-bound операция)
        return hashedFiles
            .GroupBy(x => x.Hash)
            .Where(g => g.Count() > 1) // Нам интересны только совпадения
            .Select(g => new DuplicateGroup(g.Key)
            {
                Files = g.Select(x => x.File).ToList()
            });
    }
}