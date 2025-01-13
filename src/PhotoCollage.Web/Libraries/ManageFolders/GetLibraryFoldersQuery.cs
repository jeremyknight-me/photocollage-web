using Ardalis.Result;
using PhotoCollage.Core.ValueObjects;
using PhotoCollage.Web.Helpers.Queries;

namespace PhotoCollage.Web.Libraries.ManageFolders;

internal sealed class GetLibraryFoldersQuery : IQuery
{
    public required LibraryId LibraryId { get; init; }
}

internal sealed class GetLibraryFoldersQueryHandler : IQueryHandler<GetLibraryFoldersQuery, FoldersViewModel>
{
    private readonly IQueryHandler<GetFileSystemFoldersQuery, FileSystemFoldersQueryResponse> fileSystemFoldersHandler;
    private readonly IQueryHandler<GetExcludedFoldersQuery, GetExcludedFoldersQueryResponse> excludedFoldersHandler;

    public GetLibraryFoldersQueryHandler(
        IQueryHandler<GetFileSystemFoldersQuery, FileSystemFoldersQueryResponse> fileSystemFoldersQueryHandler,
        IQueryHandler<GetExcludedFoldersQuery, GetExcludedFoldersQueryResponse> excludedFoldersQueryHandler)
    {
        this.fileSystemFoldersHandler = fileSystemFoldersQueryHandler;
        this.excludedFoldersHandler = excludedFoldersQueryHandler;
    }

    public async Task<Result<FoldersViewModel>> Handle(GetLibraryFoldersQuery query)
    {
        var fileSystemResult = await this.fileSystemFoldersHandler.Handle(new());
        var excludedFoldersResult = await this.excludedFoldersHandler.Handle(new() { LibraryId = query.LibraryId });
        List<FoldersViewModel.FolderViewModel> folders = [];
        foreach (var fileSystemFolder in fileSystemResult.Value.Folders)
        {
            var folder = this.CreateFolder(fileSystemFolder, excludedFoldersResult.Value.ExcludedFolders);
            folders.Add(folder);
        }

        FoldersViewModel viewModel = new()
        {
            Folders = [.. folders.OrderBy(x => x.Path)]
        };
        return Result.Success(viewModel);
    }

    private FoldersViewModel.FolderViewModel CreateFolder(FileSystemFoldersQueryResponse.FolderInfo folderInfo, string[] excludedFolders)
    {
        (var isExcluded, var isDisabled) = this.DetermineState(folderInfo.Path, excludedFolders);
        return new()
        {
            Name = folderInfo.Name,
            Path = folderInfo.Path,
            FolderCount = folderInfo.FolderCount,
            PhotoCount = folderInfo.PhotoCount,
            IsDisabled = isDisabled,
            IsExcluded = isExcluded
        };
    }

    private (bool IsExcluded, bool IsDisabled) DetermineState(string path, string[] excludedFolders)
    {
        path = path.TrimStart(['\\', '/']);
        foreach (var excluded in excludedFolders)
        {
            if (path == excluded)
            {
                return (true, false);
            }

            if (path.StartsWith(excluded))
            {
                return (true, true);
            }
        }

        return (false, false);
    }
}
