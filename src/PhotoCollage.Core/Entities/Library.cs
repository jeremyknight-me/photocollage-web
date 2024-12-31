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

        if (this.ExistsInExcludedFolder(relativePath))
        {
            return Result.NoContent();
        }

        if (this.excludedFolders.Any(f => f.RelativePath == relativePath))
        {
            return Result.NoContent();
        }

        var excludedFolder = ExcludedFolder.Create(relativePath);
        this.excludedFolders.Add(excludedFolder);
        return Result.Success();
    }

    public Result AddPhoto(string relativePath, long sizeBytes)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
        {
            return Result.Invalid(new ValidationError
            {
                Identifier = nameof(relativePath),
                ErrorMessage = "Photo relative path cannot be null or empty."
            });
        }

        if (this.ExistsInExcludedFolder(relativePath))
        {
            return Result.NoContent();
        }

        if (this.photos.Any(p => p.RelativePath == relativePath))
        {
            return Result.NoContent();
        }

        var photo = Photo.Create(relativePath, sizeBytes);
        this.photos.Add(photo);
        return Result.Success();
    }

    public void UpdateLastRefreshed(DateTimeOffset date)
        => this.LastRefreshed = date;

    private bool ExistsInExcludedFolder(string relativePath)
        => !string.IsNullOrWhiteSpace(relativePath)
            && this.excludedFolders.Any(f => relativePath.StartsWith(f.RelativePath));
}
