using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using PhotoCollage.Core;
using PhotoCollage.Core.Entities;
using PhotoCollage.Core.ValueObjects;
using PhotoCollage.Persistence;
using PhotoCollage.Web.Helpers.Commands;

namespace PhotoCollage.Web.Libraries.ManagePhotos;

internal sealed class ChangePhotoStatusCommand : ICommand
{
    public required PhotoId PhotoId { get; init; }
    public required PhotoStatus Status { get; init; }
}

internal sealed class ChangePhotoStatusCommandHandler : ICommandHandler<ChangePhotoStatusCommand>
{
    private readonly IDbContextFactory<PhotoCollageContext> contextFactory;

    public ChangePhotoStatusCommandHandler(IDbContextFactory<PhotoCollageContext> photoCollageContextFactory)
    {
        this.contextFactory = photoCollageContextFactory;
    }

    public async Task<Result> Handle(ChangePhotoStatusCommand request, CancellationToken cancellationToken)
    {
        using var context = this.contextFactory.CreateDbContext();
        var photo = await context.Set<Photo>()
            .FirstOrDefaultAsync(p => p.Id == request.PhotoId);
        if (photo is null)
        {
            return Result.NotFound();
        }

        photo.UpdateStatus(request.Status);
        _ = await context.SaveChangesAsync();
        return Result.Success();
    }
}
