using System.Collections.Concurrent;

namespace PhotoCollage.Web.Collages;

internal static class CollageEndpoints
{
    internal static IEndpointRouteBuilder MapCollageEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("/collage/photos/{photoId:long}", async (long photoId) =>
        {
            var path = PhotoService.GetAll()
                .OrderBy(x => Random.Shared.Next())
                .FirstOrDefault();

            // Path.Combine(PhotoService.Directory, path);
            var fullPath = PhotoService.Directory + path;
            if (!File.Exists(fullPath))
            {
                return Results.NotFound();
            }

            var extension = Path.GetExtension(fullPath);
            var mimeType = extension switch
            {
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                _ => "application/octet-stream"
            };

            var bytes = await File.ReadAllBytesAsync(fullPath);
            return Results.File(bytes, contentType: mimeType);

        });

        return builder;
    }
}

internal static class PhotoService
{
    internal const string Directory = "/data/collage";

    internal static IReadOnlyCollection<string> GetAll()
    {
        var files = System.IO.Directory.EnumerateFiles(Directory, "*", SearchOption.AllDirectories);
        var paths = GetPathsWithExtension(files);
        return paths;
    }

    private static IReadOnlyCollection<string> GetPathsWithExtension(IEnumerable<string> files)
    {
        var extensions = new HashSet<string> { ".jpg", ".jpeg", ".png" };
        var length = Directory.Length;
        var paths = new ConcurrentQueue<string>();
        var exceptions = new ConcurrentQueue<Exception>();
        Parallel.ForEach(files, file =>
        {
            try
            {
                var fileExtension = Path.GetExtension(file);
                if (extensions.Contains(fileExtension, StringComparer.OrdinalIgnoreCase))
                {
                    var path = file.Remove(0, length).TrimStart(new[] { '\\' });
                    paths.Enqueue(path);
                }
            }
            catch (Exception ex)
            {
                exceptions.Enqueue(ex);
            }
        });

        return exceptions.IsEmpty
            ? paths
            : throw new AggregateException(exceptions);
    }
}
