using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using PhotoCollage.Core.ValueObjects;
using PhotoCollage.Persistence;
using PhotoCollage.Web.Helpers.Commands;

namespace PhotoCollage.Web.Libraries.ManageFolders;

internal sealed class RemoveExcludedFolderCommand : ICommand
{
    public required LibraryId LibraryId { get; init; }
    public required string Path { get; init; }
}

internal sealed class RemoveExcludedFolderCommandHandler : ICommandHandler<RemoveExcludedFolderCommand>
{
    private readonly IDbContextFactory<PhotoCollageContext> contextFactory;

    public RemoveExcludedFolderCommandHandler(IDbContextFactory<PhotoCollageContext> photoCollageContextFactory)
    {
        this.contextFactory = photoCollageContextFactory;
    }

    public async Task<Result> Handle(RemoveExcludedFolderCommand command)
    {
        using var context = this.contextFactory.CreateDbContext();
        var library = await context.Libraries
            .Include(x => x.ExcludedFolders)
            .FirstOrDefaultAsync(x => x.Id == command.LibraryId);
        if (library is null)
        {
            return Result.NotFound();
        }

        var result = library.RemoveExcludedFolder(command.Path);
        if (result.IsSuccess)
        {
            _ = await context.SaveChangesAsync();
        }

        return result;
    }
}
