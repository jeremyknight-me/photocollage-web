using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using PhotoCollage.Persistence;
using PhotoCollage.Web.Client.Libraries;
using PhotoCollage.Web.Helpers.Queries;

namespace PhotoCollage.Web.Libraries;

internal sealed class GetLibrariesQuery : IQuery<GetLibrariesResponse>
{
}

internal sealed class GetLibrariesQueryHandler : IQueryHandler<GetLibrariesQuery, GetLibrariesResponse>
{
    private readonly IDbContextFactory<PhotoCollageContext> contextFactory;

    public GetLibrariesQueryHandler(IDbContextFactory<PhotoCollageContext> photoCollageContextFactory)
    {
        this.contextFactory = photoCollageContextFactory;
    }

    public async Task<Result<GetLibrariesResponse>> Handle(GetLibrariesQuery request, CancellationToken cancellationToken)
    {
        using var context = this.contextFactory.CreateDbContext();
        var libraries = await context.Libraries
            .Select(l => new GetLibrariesResponse.LibraryResponse()
            {
                Id = l.Id,
                Name = l.Name,
                Description = l.Description,
                RefreshedOn = l.LastRefreshed,
                ModifiedOn = l.DateModified,
                CreatedOn = l.DateCreated
            })
            .OrderBy(l => l.Name)
            .ToListAsync(cancellationToken);
        var response = new GetLibrariesResponse
        {
            Values = [.. libraries]
        };
        return Result.Success(response);
    }
}
