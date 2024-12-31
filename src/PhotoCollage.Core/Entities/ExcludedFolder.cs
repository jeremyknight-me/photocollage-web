using PhotoCollage.Core.ValueObjects;

namespace PhotoCollage.Core.Entities;

public sealed class ExcludedFolder : EntityBase<ExcludedFolderId>
{
    private ExcludedFolder()
    {
    }

    public LibraryId LibraryId { get; private set; }
    public Library Library { get; private set; } = null!;

    private string relativePath = null!;
    public required string RelativePath
    {
        get => this.relativePath;
        init => this.relativePath = value?.Trim() ?? throw new ArgumentNullException(nameof(value));
    }

    public static ExcludedFolder Create(string relativePath)
        => new() { RelativePath = relativePath };
}
