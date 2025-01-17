using Ardalis.Result;
using Microsoft.Extensions.Options;
using PhotoCollage.Core.Extensions;
using PhotoCollage.Web.Helpers.Queries;

namespace PhotoCollage.Web.Libraries.ManageFolders;

internal sealed class GetFileSystemFoldersQuery : IQuery<FileSystemFoldersQueryResponse>
{
}

internal sealed class GetFileSystemFoldersQueryHandler : IQueryHandler<GetFileSystemFoldersQuery, FileSystemFoldersQueryResponse>
{
    private readonly ApplicationSettings settings;

    public GetFileSystemFoldersQueryHandler(IOptionsSnapshot<ApplicationSettings> optionsSnapshot)
    {
        this.settings = optionsSnapshot.Value;
    }

    public async Task<Result<FileSystemFoldersQueryResponse>> Handle(GetFileSystemFoldersQuery request, CancellationToken cancellationToken)
    {
        DirectoryInfo rootDirectory = new(this.settings.PhotoRootDirectory);
        var rootLength = this.settings.PhotoRootDirectory.Length;

        var folders = new List<FileSystemFoldersQueryResponse.FolderInfo>();
        var directories = rootDirectory.EnumerateDirectories("*", SearchOption.AllDirectories);
        foreach (var directory in directories)
        {
            var folderCount = directory.EnumerateDirectories().Count();
            var photoCount = directory.EnumerateFiles().AddPhotosFilter().Count();
            folders.Add(new FileSystemFoldersQueryResponse.FolderInfo
            {
                Name = directory.Name,
                Path = directory.FullName[rootLength..],
                FolderCount = folderCount,
                PhotoCount = photoCount
            });
        }

        var response = new FileSystemFoldersQueryResponse
        {
            Folders = folders.ToArray()
        };
        var result = Result.Success(response);
        return await Task.FromResult(result);
    }
}

internal sealed class FileSystemFoldersQueryResponse
{
    public required FolderInfo[] Folders { get; init; }

    internal sealed class FolderInfo
    {
        public required int FolderCount { get; init; }
        public required string Name { get; init; }
        public required string Path { get; init; }
        public required int PhotoCount { get; init; }
    }
}
