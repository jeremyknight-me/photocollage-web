using PhotoCollage.Web.Helpers.Commands;
using PhotoCollage.Web.Helpers.Queries;
using PhotoCollage.Web.Libraries;
using PhotoCollage.Web.Libraries.Create;
using PhotoCollage.Web.Libraries.ManageFolders;
using PhotoCollage.Web.Libraries.RefreshJob;

namespace PhotoCollage.Web.Startup;

internal static class LibraryConfiguration
{
    internal static WebApplicationBuilder SetupLibrary(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddScoped<ICommandHandler<CreateLibraryCommand, int>, CreateLibraryCommandHandler>()
            .AddScoped<ICommandHandler<RefreshLibrariesCommand>, RefreshLibrariesCommandHandler>()
            .AddScoped<IQueryHandler<GetFileSystemFoldersQuery, FileSystemFoldersQueryResponse>, GetFileSystemFoldersQueryHandler>()
            .AddScoped<IQueryHandler<GetExcludedFoldersQuery, GetExcludedFoldersQueryResponse>, GetExcludedFoldersQueryHandler>()
            .AddScoped<IQueryHandler<GetLibraryFoldersQuery, FoldersViewModel>, GetLibraryFoldersQueryHandler>()
            .AddScoped<IQueryHandler<GetLibrariesQuery, GetLibrariesResponse>, GetLibrariesQueryHandler>();
        return builder;
    }
}
