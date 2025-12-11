namespace Zt.FolderSync.Core.Models;

public enum EntryStatus
{
    Identical,
    LeftOnly,
    RightOnly,
    Different,
    TypeMismatch
}