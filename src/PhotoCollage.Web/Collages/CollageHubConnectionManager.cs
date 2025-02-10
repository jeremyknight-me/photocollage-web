using System.Collections.Concurrent;

namespace PhotoCollage.Web.Collages;

internal sealed class CollageHubConnectionManager
{
    private readonly ConcurrentDictionary<int, HashSet<string>> libraryClients = [];

    public void AddClient(int libraryId, string clientId)
    {
        if (this.libraryClients.TryGetValue(libraryId, out var clients) && clients is not null)
        {
            clients.Add(clientId);
            return;
        }
        
        this.libraryClients[libraryId] = [clientId];
    }

    public int[] GetLibraries(string clientId)
        => this.libraryClients
            .SelectMany(x => x.Value
                .Where(y => y == clientId)
                .Select(y => x.Key)
            )
            .Distinct()
            .ToArray();

    public bool HasClients(int libraryId)
        => this.libraryClients.TryGetValue(libraryId, out var clients)
            && clients.Count != 0;

    public void RemoveClient(int libraryId, string clientId)
    {
        if (!this.libraryClients.TryGetValue(libraryId, out var clients) || clients is null)
        {
            return;
        }

        clients.Remove(clientId);
    }
}
