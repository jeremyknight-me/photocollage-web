using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using PhotoCollage.Core.Entities;
using PhotoCollage.Core.ValueObjects;
using PhotoCollage.Persistence;
using PhotoCollage.Web.Helpers.Queries;

namespace PhotoCollage.Web.Libraries.ManageFolders;

internal sealed class GetExcludedFoldersQuery : IQuery<GetExcludedFoldersQueryResponse>
{
    public required LibraryId LibraryId { get; init; }
}

internal sealed class GetExcludedFoldersQueryHandler : IQueryHandler<GetExcludedFoldersQuery, GetExcludedFoldersQueryResponse>
{
    private readonly IDbContextFactory<PhotoCollageContext> contextFactory;

    public GetExcludedFoldersQueryHandler(IDbContextFactory<PhotoCollageContext> photoCollageContextFactory)
    {
        this.contextFactory = photoCollageContextFactory;
    }

    public async Task<Result<GetExcludedFoldersQueryResponse>> Handle(GetExcludedFoldersQuery request, CancellationToken cancellationToken)
    {
        using var context = this.contextFactory.CreateDbContext();
        var folders = await context.Set<ExcludedFolder>()
            .Where(ef => ef.LibraryId == request.LibraryId)
            .OrderBy(ef => ef.RelativePath)
            .Select(ef => ef.RelativePath)
            .ToArrayAsync(cancellationToken);
        GetExcludedFoldersQueryResponse response = new() { ExcludedFolders = folders };
        return Result.Success(response);
    }
}

internal sealed class GetExcludedFoldersQueryResponse
{
    public required string[] ExcludedFolders { get; init; }
}
