namespace PhotoCollage.Web.Libraries.ManageFolders;

internal sealed class FoldersViewModel
{
    private const string iconClassesDisabled = "bi-toggle-off text-secondary";
    private const string iconClassesExcluded = "bi-toggle-off text-danger";
    private const string iconClassesIncluded = "bi-toggle-on text-success";

    public required FolderViewModel[] Folders { get; init; }

    internal sealed class FolderViewModel
    {
        public required int FolderCount { get; init; }
        public required string Name { get; init; }
        public required string Path { get; init; }
        public required int PhotoCount { get; init; }
        public required bool IsExcluded { get; init; }
        public required bool IsDisabled { get; init; }

        public string IconClasses
        {
            get
            {
                if (this.IsDisabled)
                {
                    return iconClassesDisabled;
                }

                return this.IsExcluded
                    ? iconClassesExcluded
                    : iconClassesIncluded;
            }
        }
    }
}
