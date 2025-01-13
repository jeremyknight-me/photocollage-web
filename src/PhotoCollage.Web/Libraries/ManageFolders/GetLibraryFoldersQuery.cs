using Ardalis.Result;
using MediatR;
using PhotoCollage.Core.ValueObjects;
using PhotoCollage.Web.Helpers.Queries;

namespace PhotoCollage.Web.Libraries.ManageFolders;

internal sealed class GetLibraryFoldersQuery : IQuery<FoldersViewModel>
{
    public required LibraryId LibraryId { get; init; }
}

internal sealed class GetLibraryFoldersQueryHandler : IQueryHandler<GetLibraryFoldersQuery, FoldersViewModel>
{
    private readonly ISender sender;

    public GetLibraryFoldersQueryHandler(ISender sender)
    {
        this.sender = sender;
    }

    public async Task<Result<FoldersViewModel>> Handle(GetLibraryFoldersQuery request, CancellationToken cancellationToken)
    {
        var fileSystemResult = await this.sender.Send(new GetFileSystemFoldersQuery(), cancellationToken);
        var excludedFoldersResult = await this.sender.Send(new GetExcludedFoldersQuery { LibraryId = request.LibraryId }, cancellationToken);
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
