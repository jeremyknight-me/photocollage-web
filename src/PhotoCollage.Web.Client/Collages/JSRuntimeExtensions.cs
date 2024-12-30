using Microsoft.JSInterop;

namespace PhotoCollage.Web.Client.Collages;

internal static class JSRuntimeExtensions
{
    internal static async Task AddPhoto(this IJSRuntime js, CollagePhotoResponse response, int index, CollageSettings settings)
            => await js.InvokeVoidAsync("collage.addPhoto", response, index, settings);

    internal static async Task RemovePhoto(this IJSRuntime js, Guid photoId)
        => await js.InvokeVoidAsync("collage.removePhoto", photoId);
}
