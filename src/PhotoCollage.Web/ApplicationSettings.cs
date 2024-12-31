using System.ComponentModel.DataAnnotations;

namespace PhotoCollage.Web;

internal sealed class ApplicationSettings
{
    public const string Path = "ApplicationSettings";

    [Required]
    public string PhotoRootDirectory { get; init; } = null!;

    public LibraryRefreshJobSettings LibraryRefreshJob { get; init; } = new();

    internal sealed class LibraryRefreshJobSettings
    {
        public bool Enabled { get; init; } = false;
    }
}
