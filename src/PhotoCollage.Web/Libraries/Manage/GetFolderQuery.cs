using Ardalis.Result;
using Microsoft.Extensions.Options;
using PhotoCollage.Core.Extensions;
using PhotoCollage.Core.ValueObjects;
using PhotoCollage.Web.Helpers.Queries;

namespace PhotoCollage.Web.Libraries.Manage;

internal sealed class GetFolderQuery : IQuery<GetFolderQueryResponse>
{
    public required string Path { get; init; }
}

internal sealed class GetFolderQueryHandler : IQueryHandler<GetFolderQuery, GetFolderQueryResponse>
{
    private readonly ApplicationSettings settings;

    public GetFolderQueryHandler(IOptionsSnapshot<ApplicationSettings> optionsSnapshot)
    {
        this.settings = optionsSnapshot.Value;
    }

    public async Task<Result<GetFolderQueryResponse>> Handle(GetFolderQuery request, CancellationToken cancellationToken)
    {
        var path = Path.Combine(this.settings.PhotoRootDirectory, request.Path);
        var isRoot = path == this.settings.PhotoRootDirectory;
        var rootLength = this.settings.PhotoRootDirectory.Length;
        var directory = new DirectoryInfo(path);

        var directories = directory.EnumerateDirectories()
            .Select(GetFolderQueryResponse.PathFolder.Create)
            .ToArray();
        var response = new GetFolderQueryResponse
        {
            Name = isRoot ? "root" : directory.Name,
            Path = isRoot ? "/" : directory.FullName[rootLength..],
            Parent = isRoot ? null : directory.Parent?.FullName,
            Folders = directories,
            Files = directory
                .EnumerateFiles("*.*", SearchOption.TopDirectoryOnly)
                .GetPhotoFiles(rootLength)
        };
        var result = Result.Success(response);
        return await Task.FromResult(result);
    }
}

internal sealed class GetFolderQueryResponse
{
    public required string Name { get; init; }
    public required string Path { get; init; }
    public required string? Parent { get; init; }
    public required IReadOnlyCollection<PathFolder> Folders { get; init; }
    public required IReadOnlyCollection<PhotoFile> Files { get; init; }

    internal sealed class PathFolder
    {
        public required string Name { get; init; }
        public required string Path { get; init; }

        internal static PathFolder Create(DirectoryInfo directory)
        {
            return new()
            {
                Name = directory.Name,
                Path = directory.FullName
            };
        }
    }
}
