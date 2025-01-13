using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using PhotoCollage.Persistence;
using PhotoCollage.Web.Helpers.Queries;

namespace PhotoCollage.Web.Collages;

internal sealed class GetLibraryPhotosQuery : IQuery<long[]>
{
}

internal sealed class GetLibraryPhotosQueryHandler : IQueryHandler<GetLibraryPhotosQuery, long[]>
{
    private readonly IDbContextFactory<PhotoCollageContext> contextFactory;

    public GetLibraryPhotosQueryHandler(IDbContextFactory<PhotoCollageContext> photoCollageContextFactory)
    {
        this.contextFactory = photoCollageContextFactory;
    }

    public async Task<Result<long[]>> Handle(GetLibraryPhotosQuery request, CancellationToken cancellationToken)
    {
        using var context = this.contextFactory.CreateDbContext();
        var library = await context.Libraries.AsNoTracking()
            .Include(l => l.Photos)
            .FirstOrDefaultAsync(cancellationToken);

        if (library is null)
        {
            return Result.Success(Array.Empty<long>());
        }

        var photoIds = library.Photos
            .Where(x => x.Status == Core.PhotoStatus.Sync)
            .Select(x => x.Id.Value)
            .ToArray();
        return Result.Success(photoIds);
    }
}
