namespace PhotoCollage.Web.Photos;

public static class PhotoEndpoints
{
    internal static IEndpointRouteBuilder MapPhotoEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("/api/photos/{photoId}", async (Guid photoId) =>
        {
            var path = PhotoService.GetAll()
                .OrderBy(x => Random.Shared.Next())
                .FirstOrDefault();

            var fullPath = PhotoService.Directory + path;// Path.Combine(PhotoService.Directory, path);
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
