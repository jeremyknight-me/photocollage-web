namespace PhotoCollage.Web.Client.Libraries;

public sealed class GetLibrariesResponse
{
    public required LibraryResponse[] Values { get; init; }

    public sealed class LibraryResponse
    {
        public required int Id { get; init; }
        public required string Name { get; init; }
        public required string? Description { get; init; }
        public required DateTimeOffset? RefreshedOn { get; init; }
        public required DateTimeOffset CreatedOn { get; init; }
        public required DateTimeOffset ModifiedOn { get; init; }
    }
}
