using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using PhotoCollage.Core.ValueObjects;
using PhotoCollage.Persistence;
using PhotoCollage.Web.Helpers.Commands;

namespace PhotoCollage.Web.Libraries.Manage;

internal sealed class GetLibraryQuery : ICommand
{
    public required LibraryId LibraryId { get; init; }
}

internal sealed class GetLibraryQueryHandler : ICommandHandler<GetLibraryQuery, GetLibraryResponse>
{
    private readonly IDbContextFactory<PhotoCollageContext> contextFactory;

    public GetLibraryQueryHandler(IDbContextFactory<PhotoCollageContext> photoCollageContextFactory)
    {
        this.contextFactory = photoCollageContextFactory;
    }

    public async Task<Result<GetLibraryResponse>> Handle(GetLibraryQuery command)
    {
        using var context = this.contextFactory.CreateDbContext();

        var library = await context.Libraries
            .FirstOrDefaultAsync(x => x.Id == command.LibraryId);

        GetLibraryResponse response = new();
        return Result.Success(response);
    }
}

internal sealed class GetLibraryResponse
{
}
