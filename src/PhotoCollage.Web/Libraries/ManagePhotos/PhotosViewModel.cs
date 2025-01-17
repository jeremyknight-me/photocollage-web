using PhotoCollage.Core;
using PhotoCollage.Core.ValueObjects;

namespace PhotoCollage.Web.Libraries.ManagePhotos;

internal sealed class PhotosViewModel
{
    public required FolderViewModel[] Folders { get; init; }

    internal sealed class FolderViewModel
    {
        public required PhotoViewModel[] Photos { get; init; }
        public required string Path { get; init; }
        public int PhotoCount => this.Photos.Length;
        public int IgnoredCount => this.Photos.Count(p => p.Status == PhotoStatus.Ignore);
        public int NewCount => this.Photos.Count(p => p.Status == PhotoStatus.New);
        public int SyncCount => this.Photos.Count(p => p.Status == PhotoStatus.Sync);
    }

    internal sealed class PhotoViewModel
    {
        public PhotoViewModel(PhotoStatus status)
        {
            this.Status = status;
        }

        public required PhotoId PhotoId { get; init; }
        public required string Extension { get; init; }
        public required string Name { get; init; }
        public required string Path { get; init; }
        public required long SizeBytes { get; init; }
        public PhotoStatus Status { get; private set; }

        public bool IsNew => this.Status == PhotoStatus.New;
        public bool IsSync => this.Status == PhotoStatus.Sync;
        public bool IsIgnored => this.Status == PhotoStatus.Ignore;

        public bool IsSaving { get; private set; } = false;

        public void SetStatus(PhotoStatus status)
            => this.Status = status;

        public void SavingStarted() => this.IsSaving = true;
        public void SavingCompleted() => this.IsSaving = false;
    }
}
