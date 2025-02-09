using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace PhotoCollage.Web.Libraries;

internal static class LibraryEndpoints
{
    internal static IEndpointRouteBuilder MapLibraryEndpoints(this IEndpointRouteBuilder builder)
    {
        builder
            .MapGet("/api/libraries", async Task<IResult> ([FromServices] ISender sender) =>
            {
                GetLibrariesQuery query = new();
                var result = await sender.Send(query);
                return result.IsSuccess
                    ? Results.Ok(result.Value)
                    : Results.BadRequest();
            });

        return builder;
    }
}
