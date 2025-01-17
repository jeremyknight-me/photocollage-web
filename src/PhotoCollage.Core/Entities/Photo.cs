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
    public required string Extension { get; init; }
    public PhotoStatus Status { get; private set; } = PhotoStatus.New;
    public long SizeBytes { get; private set; } = 0;
    public string Name => Path.GetFileName(this.RelativePath);

    public void UpdateStatus(PhotoStatus status) => this.Status = status;
    public void UpdateSizeBytes(long size) => this.SizeBytes = size;

    public static Photo Create(string relativePath, string extension, long sizeBytes)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(relativePath);
        return new()
        {
            RelativePath = relativePath.Trim(),
            Extension = extension.Trim().ToUpper(),
            SizeBytes = sizeBytes
        };
    }
}
