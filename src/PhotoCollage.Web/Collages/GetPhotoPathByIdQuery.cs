using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using PhotoCollage.Core.Entities;
using PhotoCollage.Core.ValueObjects;
using PhotoCollage.Persistence;
using PhotoCollage.Web.Helpers.Queries;

namespace PhotoCollage.Web.Collages;

internal sealed class GetPhotoPathByIdQuery : IQuery<string>
{
    public required PhotoId PhotoId { get; init; }
}

internal sealed class GetPhotoPathByIdQueryHandler : IQueryHandler<GetPhotoPathByIdQuery, string>
{
    private readonly IDbContextFactory<PhotoCollageContext> contextFactory;

    public GetPhotoPathByIdQueryHandler(IDbContextFactory<PhotoCollageContext> photoCollageContextFactory)
    {
        this.contextFactory = photoCollageContextFactory;
    }

    public async Task<Result<string>> Handle(GetPhotoPathByIdQuery request, CancellationToken cancellationToken)
    {
        using var context = this.contextFactory.CreateDbContext();
        var photo = await context.Set<Photo>().FirstOrDefaultAsync(p => p.Id == request.PhotoId);
        return photo is null
            ? (Result<string>)Result.NotFound()
            : Result.Success(photo.RelativePath);
    }
}
