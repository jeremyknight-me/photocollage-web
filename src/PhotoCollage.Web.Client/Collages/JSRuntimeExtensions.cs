using Microsoft.JSInterop;

namespace PhotoCollage.Web.Client.Collages;

internal static class JSRuntimeExtensions
{
    internal static async Task AddPhoto(this IJSRuntime js, long id, string url, int index, CollageSettings settings)
            => await js.InvokeVoidAsync("collage.addPhoto", id, url, index, settings);

    internal static async Task RemovePhoto(this IJSRuntime js, long photoId)
        => await js.InvokeVoidAsync("collage.removePhoto", photoId);
}
