using PhotoCollage.Web.Helpers.Commands;
using PhotoCollage.Web.Helpers.Queries;
using PhotoCollage.Web.Libraries;
using PhotoCollage.Web.Libraries.Create;

namespace PhotoCollage.Web.Startup;

internal static class LibraryConfiguration
{
    internal static WebApplicationBuilder SetupLibrary(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddScoped<IQueryHandler<GetLibrariesQuery, IReadOnlyCollection<LibraryResponse>>, GetLibrariesQueryHandler>()
            .AddScoped<ICommandHandler<CreateLibraryCommand, int>, CreateLibraryCommandHandler>();
        return builder;
    }
}
