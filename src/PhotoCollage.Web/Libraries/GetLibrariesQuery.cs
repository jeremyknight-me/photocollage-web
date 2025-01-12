using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using PhotoCollage.Persistence;
using PhotoCollage.Web.Helpers.Queries;

namespace PhotoCollage.Web.Libraries;

internal sealed class GetLibrariesQuery : IQuery
{
}

internal sealed class GetLibrariesQueryHandler : IQueryHandler<GetLibrariesQuery, GetLibrariesResponse>
{
    private readonly IDbContextFactory<PhotoCollageContext> contextFactory;

    public GetLibrariesQueryHandler(IDbContextFactory<PhotoCollageContext> photoCollageContextFactory)
    {
        this.contextFactory = photoCollageContextFactory;
    }

    public async Task<Result<GetLibrariesResponse>> Handle(GetLibrariesQuery command)
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
            .ToListAsync();
        var response = new GetLibrariesResponse
        {
            Values = libraries
        };
        return Result.Success(response);
    }
}

internal sealed class GetLibrariesResponse
{
    public required IReadOnlyCollection<LibraryResponse> Values { get; init; }

    internal sealed class LibraryResponse
    {
        public required int Id { get; init; }
        public required string Name { get; init; }
        public required string? Description { get; init; }
        public required DateTimeOffset? RefreshedOn { get; init; }
        public required DateTimeOffset CreatedOn { get; init; }
        public required DateTimeOffset ModifiedOn { get; init; }
    }
}
