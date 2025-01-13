using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace PhotoCollage.Web.Collages;

internal static class CollageEndpoints
{
    internal static IEndpointRouteBuilder MapCollageEndpoints(this IEndpointRouteBuilder builder)
    {
        builder
            .MapGet("/collage/photos/{photoId:long}", async Task<Microsoft.AspNetCore.Http.IResult> (
                [FromRoute] long photoId,
                [FromServices] ISender sender,
                [FromServices] IOptionsSnapshot<ApplicationSettings> options
                ) =>
            {
                var query = new GetPhotoPathByIdQuery { PhotoId = photoId };
                var pathResult = await sender.Send(query);
                if (pathResult.IsNotFound())
                {
                    return Results.NotFound();
                }

                var settings = options.Value;
                var fullPath = Path.Combine(settings.PhotoRootDirectory, pathResult.Value);
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
            })
            .AddEndpointFilter(async (context, next) =>
            {
                context.HttpContext.Response.Headers.Append("Cache-Control", "no-cache");
                var result = await next(context);
                return result;
            });

        return builder;
    }
}
