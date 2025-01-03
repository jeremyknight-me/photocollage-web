using PhotoCollage.Core.ValueObjects;

namespace PhotoCollage.Core.Entities;

public sealed class Photo : EntityBase<PhotoId>
{
    public enum PhotoStatus
    {
        New = 0,
        Sync = 1,
        Ignore = 2
    }

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

    public bool IsIgnored => this.Status == PhotoStatus.Ignore;
    public bool IsNew => this.Status == PhotoStatus.New;
    public bool IsSynced => this.Status == PhotoStatus.Sync;

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
