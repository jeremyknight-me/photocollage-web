using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using PhotoCollage.Core.ValueObjects;
using PhotoCollage.Persistence;
using PhotoCollage.Web.Helpers.Commands;

namespace PhotoCollage.Web.Libraries.ManageFolders;

internal sealed class AddExcludedFolderCommand : ICommand
{
    public required LibraryId LibraryId { get; init; }
    public required string Path { get; init; }
}

internal sealed class AddExcludedFolderCommandHandler : ICommandHandler<AddExcludedFolderCommand>
{
    private readonly IDbContextFactory<PhotoCollageContext> contextFactory;

    public AddExcludedFolderCommandHandler(IDbContextFactory<PhotoCollageContext> photoCollageContextFactory)
    {
        this.contextFactory = photoCollageContextFactory;
    }

    public async Task<Result> Handle(AddExcludedFolderCommand command)
    {
        using var context = this.contextFactory.CreateDbContext();
        var library = await context.Libraries
            .Include(x => x.ExcludedFolders)
            .FirstOrDefaultAsync(x => x.Id == command.LibraryId);
        if (library is null)
        {
            return Result.NotFound();
        }

        var result = library.AddExcludedFolder(command.Path);
        if (result.IsSuccess)
        {
            _ = await context.SaveChangesAsync();
        }

        return result;
    }
}
