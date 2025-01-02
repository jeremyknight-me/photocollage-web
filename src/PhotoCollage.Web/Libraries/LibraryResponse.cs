namespace PhotoCollage.Web.Libraries;

internal sealed class LibraryResponse
{
    public required string Name { get; init; }
    public required string? Description { get; init; }
    public required DateTimeOffset? RefreshedOn { get; init; }
    public required DateTimeOffset CreatedOn { get; init; }
    public required DateTimeOffset ModifiedOn { get; init; }
}
