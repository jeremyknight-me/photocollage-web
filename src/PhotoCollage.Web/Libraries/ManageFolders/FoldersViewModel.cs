namespace PhotoCollage.Web.Libraries.ManageFolders;

internal sealed class FoldersViewModel
{
    //private readonly List<FolderViewModel> folders = [];

    //public FolderViewModel[] Folders => this.folders.ToArray();

    internal sealed class FolderViewModel
    {
        public FolderViewModel(bool isExcluded)
        {
            this.IsExcluded = isExcluded;
        }

        public required int FolderCount { get; init; }
        public required string Name { get; init; }
        public required string Path { get; init; }
        public required int PhotoCount { get; init; }
        public bool IsExcluded { get; private set; }
    }
}
