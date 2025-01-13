using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using PhotoCollage.Core.Entities;
using PhotoCollage.Core.ValueObjects;
using PhotoCollage.Persistence;
using PhotoCollage.Web.Helpers.Queries;

namespace PhotoCollage.Web.Libraries.ManagePhotos;

internal sealed class GetLibraryPhotosQuery : IQuery<PhotosViewModel>
{
    public required LibraryId LibraryId { get; init; }
}

internal sealed class GetLibraryPhotosQueryHandler : IQueryHandler<GetLibraryPhotosQuery, PhotosViewModel>
{
    private readonly IDbContextFactory<PhotoCollageContext> contextFactory;

    public GetLibraryPhotosQueryHandler(IDbContextFactory<PhotoCollageContext> photoCollageContextFactory)
    {
        this.contextFactory = photoCollageContextFactory;
    }

    public async Task<Result<PhotosViewModel>> Handle(GetLibraryPhotosQuery request, CancellationToken cancellationToken)
    {
        using var context = this.contextFactory.CreateDbContext();
        var photos = await context.Set<Photo>().AsNoTracking()
            .Where(p => p.LibraryId == request.LibraryId)
            .OrderBy(p => p.RelativePath)
            .ToArrayAsync(cancellationToken);

        var currentFolderPath = string.Empty;
        List<PhotosViewModel.FolderViewModel> folderModels = [];
        List<PhotosViewModel.PhotoViewModel> currentFolderPhotos = [];
        foreach (var photo in photos)
        {
            var folderPath = Path.GetDirectoryName(photo.RelativePath);
            if (string.IsNullOrWhiteSpace(currentFolderPath))
            {
                currentFolderPath = folderPath;
            }

            if (currentFolderPath != folderPath)
            {
                var folderModel = new PhotosViewModel.FolderViewModel
                {
                    Path = currentFolderPath,
                    Photos = currentFolderPhotos.ToArray()
                };
                folderModels.Add(folderModel);
                currentFolderPath = folderPath;
                currentFolderPhotos.Clear();
            }

            var photoModel = new PhotosViewModel.PhotoViewModel(photo.Status)
            {
                PhotoId = photo.Id,
                Extension = photo.Extension,
                Name = photo.Name,
                Path = photo.RelativePath,
                SizeBytes = photo.SizeBytes
            };
            currentFolderPhotos.Add(photoModel);
        }

        var model = new PhotosViewModel
        {
            Folders = [.. folderModels]
        };
        return Result.Success(model);
    }
}
