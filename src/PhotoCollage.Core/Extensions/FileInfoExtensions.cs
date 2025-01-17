using PhotoCollage.Core.ValueObjects;

namespace PhotoCollage.Core.Extensions;

public static class FileInfoExtensions
{
    public static PhotoFile[] GetPhotoFiles(this IEnumerable<FileInfo> files, int length)
        => files
            .AddPhotosFilter()
            .Select(fi => PhotoFile.Create(
                relativePath: fi.FullName[length..],
                extension: fi.Extension,
                sizeInBytes: fi.Length))
            .ToArray();

    public static IEnumerable<FileInfo> AddPhotosFilter(this IEnumerable<FileInfo> files)
        => files
             .Where(f =>
                f.Extension.Equals(".jpg", StringComparison.OrdinalIgnoreCase)
                || f.Extension.Equals(".jpeg", StringComparison.OrdinalIgnoreCase)
                || f.Extension.Equals(".png", StringComparison.OrdinalIgnoreCase));
}
