namespace PhotoCollage.Core.ValueObjects;

public sealed class PhotoFile
{
    private PhotoFile()
    {
    }

    public required string Extension { get; init; }
    public required string RelativePath { get; init; }
    public required long SizeInBytes { get; init; }

    public static PhotoFile Create(string relativePath, string extension, long sizeInBytes)
        => new()
        {
            RelativePath = relativePath.Trim().TrimStart(['\\', '/']),
            Extension = extension.Trim().TrimStart('.').ToUpper(),
            SizeInBytes = sizeInBytes
        };
}
