using PhotoCollage.Core.ValueObjects;

namespace PhotoCollage.Core.Entities;

public sealed class Library : EntityBase<LibraryId>
{
    private readonly List<ExcludedFolder> excludedFolders = [];
    private readonly List<Photo> photos = [];

    public required string Name { get; set; }

    public IReadOnlyCollection<ExcludedFolder> ExcludedFolders => this.excludedFolders.ToList();
    public IReadOnlyCollection<Photo> Photos => this.photos.ToList();
}
