using Ardalis.Result;
using PhotoCollage.Core.ValueObjects;

namespace PhotoCollage.Core.Entities;

public sealed class Library : EntityBase<LibraryId>
{
    private readonly List<ExcludedFolder> excludedFolders = [];
    private readonly List<Photo> photos = [];

    private Library()
    {
    }

    public required string Name { get; set; }
    public DateTimeOffset? LastRefreshed { get; private set; } = null;

    public IReadOnlyCollection<ExcludedFolder> ExcludedFolders => this.excludedFolders.ToList();
    public IReadOnlyCollection<Photo> Photos => this.photos.ToList();

    public static Library Create(string name)
        => new() { Name = name };

    public Result AddExcludedFolder(string relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
        {
            return Result.Invalid(new ValidationError
            {
                Identifier = nameof(relativePath),
                ErrorMessage = "Folder relative path cannot be null or empty."
            });
        }

        relativePath = relativePath.TrimStart(['\\', '/']);
        if (this.excludedFolders.Any(f => f.RelativePath == relativePath))
        {
            return Result.Invalid(new ValidationError
            {
                Identifier = nameof(relativePath),
                ErrorMessage = "Folder relative path cannot be duplicated."
            });
        }

        if (this.ExistsInExcludedFolder(relativePath))
        {
            return Result.NoContent();
        }

        var excludedFolder = ExcludedFolder.Create(relativePath);
        this.excludedFolders.Add(excludedFolder);
        return Result.Success();
    }

    public Result Refresh(IReadOnlyCollection<PhotoFile> files)
    {
        _ = this.MergeExcludedFolders();
        _ = this.RemoveExcludedPhotos();
        _ = this.RefreshPhotos(files);
        return Result.Success();
    }

    internal Result AddPhoto(PhotoFile file)
    {
        if (string.IsNullOrWhiteSpace(file.RelativePath))
        {
            return Result.Invalid(new ValidationError
            {
                Identifier = nameof(file.RelativePath),
                ErrorMessage = "Photo relative path cannot be null or empty."
            });
        }

        if (this.photos.Any(p => p.RelativePath == file.RelativePath))
        {
            return Result.Invalid(new ValidationError
            {
                Identifier = nameof(file.RelativePath),
                ErrorMessage = "Photo relative path cannot be duplicated."
            });
        }

        if (this.ExistsInExcludedFolder(file.RelativePath))
        {
            return Result.NoContent();
        }

        var photo = Photo.Create(file.RelativePath, file.Extension, file.SizeInBytes);
        this.photos.Add(photo);
        return Result.Success();
    }

    private bool ExistsInExcludedFolder(string relativePath)
        => !string.IsNullOrWhiteSpace(relativePath)
            && this.excludedFolders.Any(f => relativePath.StartsWith(f.RelativePath));

    private void DeletePhotosNotIn(IReadOnlyCollection<PhotoFile> files)
    {
        if (this.photos.Count == 0)
        {
            return;
        }

        _ = this.photos.RemoveAll(p => !files.Any(f => p.RelativePath == f.RelativePath));
    }

    private Result MergeExcludedFolders()
    {
        if (this.excludedFolders.Count == 0)
        {
            return Result.NoContent();
        }

        var folderIdsToRemove = this.excludedFolders
            .Where(folderToCheck =>
                this.excludedFolders.Any(ef => folderToCheck.RelativePath.Length > ef.RelativePath.Length
                    && folderToCheck.RelativePath.StartsWith(ef.RelativePath)
                )
            )
            .Select(f => f.Id);

        foreach (var folderId in folderIdsToRemove)
        {
            var folder = this.excludedFolders.FirstOrDefault(f => f.Id == folderId);
            if (folder is not null)
            {
                _ = this.excludedFolders.Remove(folder);
            }
        }

        return Result.Success();
    }

    private Result RefreshPhotos(IReadOnlyCollection<PhotoFile> files)
    {
        this.DeletePhotosNotIn(files);
        foreach (var file in files)
        {
            var photo = this.photos.FirstOrDefault(p => p.RelativePath == file.RelativePath);
            if (photo is null)
            {
                _ = this.AddPhoto(file);
            }
            else
            {
                photo.UpdateSizeBytes(file.SizeInBytes);
            }
        }

        this.LastRefreshed = DateTimeOffset.Now;
        return Result.Success();
    }

    private Result RemoveExcludedPhotos()
    {
        if (this.excludedFolders.Count == 0)
        {
            return Result.NoContent();
        }

        var folders = this.excludedFolders.Select(x => x.RelativePath).ToArray();
        _ = this.photos.RemoveAll(p => folders.Any(f => p.RelativePath.StartsWith(f)));
        return Result.Success();
    }
}
