using Microsoft.AspNetCore.SignalR;
using PhotoCollage.Web.Client.Collages;

namespace PhotoCollage.Web.Collages;

public interface ICollageClient
{
    Task ReceivePhoto(Guid photoId);
    Task ReceiveRemove(Guid photoId);
    Task ReceiveConnected(CollageSettings settings);
}

internal sealed class CollageHub : Hub<ICollageClient>
{
    public const string ConnectedGroupName = "connected";
    private readonly CollageSettings settings = new();
    private readonly CollageHubConnectionManager connectionManager;

    public CollageHub(CollageHubConnectionManager connectionManager)
    {
        this.connectionManager = connectionManager;
    }

    public override async Task OnConnectedAsync()
    {
        this.connectionManager.AddClient(this.Context.ConnectionId);
        await this.Groups.AddToGroupAsync(this.Context.ConnectionId, ConnectedGroupName);
        await this.Clients.Caller.ReceiveConnected(this.settings);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await this.Groups.RemoveFromGroupAsync(this.Context.ConnectionId, ConnectedGroupName);
        this.connectionManager.RemoveClient(this.Context.ConnectionId);
    }
}
