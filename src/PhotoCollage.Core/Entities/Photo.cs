using PhotoCollage.Core.Enums;
using PhotoCollage.Core.ValueObjects;

namespace PhotoCollage.Core.Entities;

public sealed class Photo : EntityBase<PhotoId>
{
    private Photo()
    {
    }

    public LibraryId LibraryId { get; private set; }
    public Library Library { get; private set; } = null!;
    public required string RelativePath { get; init; }
    public string Extension { get; private init; } = null!;
    public PhotoAction ProcessAction { get; private set; } = PhotoAction.New;
    public long SizeBytes { get; private set; } = 0;
    public string Name => Path.GetFileName(this.RelativePath);

    public void UpdateProcessAction(PhotoAction action) => this.ProcessAction = action;
    public void UpdateSizeBytes(long size) => this.SizeBytes = size;

    public static Photo Create(string relativePath, long sizeBytes)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(relativePath);
        return new()
        {
            RelativePath = relativePath.Trim(),
            Extension = Path.GetExtension(relativePath).Trim('.'),
            SizeBytes = sizeBytes
        };
    }
}
