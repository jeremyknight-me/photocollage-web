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
        var fileSystemFoldersResult = await this.fileSystemFoldersHandler.Handle(new());
        var excludedFoldersResult = await this.excludedFoldersHandler.Handle(new() { LibraryId = query.LibraryId });

        List<FoldersViewModel.FolderViewModel> folderViewModels = [];
        foreach (var fileSystemFolder in fileSystemFoldersResult.Value.Folders)
        {
            var isExcluded = false; // todo: determine if in excluded path
            FoldersViewModel.FolderViewModel vm = new(isExcluded)
            {
                Name = fileSystemFolder.Name,
                Path = fileSystemFolder.Path,
                FolderCount = fileSystemFolder.FolderCount,
                PhotoCount = fileSystemFolder.PhotoCount,
            };
            folderViewModels.Add(vm);
        }

        FoldersViewModel viewModel = new()
        {
        };
        return Result.Success(viewModel);
    }
}
