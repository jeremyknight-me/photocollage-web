using PhotoCollage.Web.Helpers.Commands;

namespace PhotoCollage.Web.Libraries.Create;

internal sealed class CreateLibraryCommand : ICommand
{
    internal required string Name { get; init; }
    internal string? Description { get; init; }
}
