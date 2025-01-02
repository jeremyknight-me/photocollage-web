using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using PhotoCollage.Persistence;
using PhotoCollage.Web.Helpers.Queries;

namespace PhotoCollage.Web.Libraries;

internal sealed class GetLibrariesQueryHandler
    : IQueryHandler<GetLibrariesQuery, IReadOnlyCollection<LibraryResponse>>
{
    private readonly IDbContextFactory<PhotoCollageContext> contextFactory;

    public GetLibrariesQueryHandler(IDbContextFactory<PhotoCollageContext> photoCollageContextFactory)
    {
        this.contextFactory = photoCollageContextFactory;
    }

    public async Task<Result<IReadOnlyCollection<LibraryResponse>>> Handle(GetLibrariesQuery command)
    {
        using var context = this.contextFactory.CreateDbContext();
        var libraries = await context.Libraries
            .Select(l => new LibraryResponse()
            {
                Name = l.Name,
                Description = l.Description,
                RefreshedOn = l.LastRefreshed,
                ModifiedOn = l.DateModified,
                CreatedOn = l.DateCreated
            })
            .OrderBy(l => l.Name)
            .ToListAsync();
        return Result.Success<IReadOnlyCollection<LibraryResponse>>(libraries);
    }
}
