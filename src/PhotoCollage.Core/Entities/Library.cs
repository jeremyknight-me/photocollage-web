using PhotoCollage.Core.ValueObjects;

namespace PhotoCollage.Core.Entities;

public sealed class Library : EntityBase<LibraryId>
{
    private readonly List<ExcludedFolder> excludedFolders = [];
    private readonly List<Photo> photos = [];

    public required string Name { get; set; }

    public IReadOnlyList<ExcludedFolder> ExcludedFolders
    {
        get => this.excludedFolders.AsReadOnly();
        init
        {
            this.excludedFolders.Clear();
            this.excludedFolders.AddRange(value);
        }
    }

    public IReadOnlyList<Photo> Photos
    {
        get => this.photos.AsReadOnly();
        init
        {
            this.photos.Clear();
            this.photos.AddRange(value);
        }
    }
}
