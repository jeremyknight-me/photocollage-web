using PhotoCollage.Web.Photos;

namespace PhotoCollage.Web.Startup;

internal static class EndpointConfiguration
{
    internal static IEndpointRouteBuilder MapCustomEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapPhotoEndpoints();
        return builder;
    }
}
