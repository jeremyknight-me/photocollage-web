using PhotoCollage.Core.Enums;
using PhotoCollage.Core.ValueObjects;

namespace PhotoCollage.Core.Entities;

public sealed class Photo : EntityBase<PhotoId>
{
    public LibraryId LibraryId { get; private set; }
    public Library Library { get; private set; } = null!;

    private string relativePath = null!;
    public required string RelativePath
    {
        get => this.relativePath;
        init => this.relativePath = value?.Trim() ?? throw new ArgumentNullException(nameof(value));
    }

    public PhotoAction ProcessAction { get; private set; } = PhotoAction.New;
    public long SizeBytes { get; private set; } = 0;
    public string Name => Path.GetFileName(this.RelativePath);

    public void UpdateProcessAction(PhotoAction action) => this.ProcessAction = action;
    public void UpdateSizeBytes(long size) => this.SizeBytes = size;

    public static Photo Create(string relativePath, long sizeBytes)
        => new()
        {
            RelativePath = relativePath,
            SizeBytes = sizeBytes
        };
}
