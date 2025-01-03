namespace PhotoCollage.Web.Libraries.Manage;

internal sealed class LibraryPathViewModel
{
    public required string Name { get; init; }
    public required string Path { get; init; }
    public required IReadOnlyDictionary<int, string> Breadcrumbs { get; init; }
    public required IReadOnlyCollection<LibraryFolder> Folders { get; init; }
    public required IReadOnlyCollection<LibraryPhoto> Photos { get; init; }

    internal sealed class LibraryFolder
    {
        public required string Name { get; init; }
        public required string Path { get; init; }
        public required bool IsExcluded { get; init; }
    }

    internal sealed class LibraryPhoto
    {
        public required string Name { get; init; }
        public required string Extension { get; init; }
        public required string Path { get; init; }

        public required long? PhotoId { get; init; }
        public required bool IsNew { get; init; }
        public required bool IsIgnored { get; init; }
    }
}
