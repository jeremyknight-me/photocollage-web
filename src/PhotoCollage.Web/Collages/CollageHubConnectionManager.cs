namespace PhotoCollage.Web.Collages;

internal sealed class CollageHubConnectionManager
{
    private readonly HashSet<string> clientIds = [];

    public IReadOnlySet<string> ClientIds => this.clientIds.ToHashSet();
    public bool HasClients => this.clientIds.Count != 0;

    public void AddClient(string clientId)
        => this.clientIds.Add(clientId);

    public void RemoveClient(string clientId)
        => this.clientIds.Remove(clientId);
}
