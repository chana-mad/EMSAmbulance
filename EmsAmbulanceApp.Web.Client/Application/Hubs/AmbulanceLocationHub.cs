using Microsoft.AspNetCore.SignalR;

namespace EmsAmbulanceApp.Web.Client.Application.Hubs;

public class AmbulanceLocationHub : Hub
{
    public async Task SendLocation(string ambulanceId, double latitude, double longitude)
    {
        // Broadcast the location update to all connected clients except the sender
        await Clients.Others.SendAsync("ReceiveLocation", ambulanceId, latitude, longitude);
    }

    public async Task TrackLocation(string requestId, string ambulanceId, double latitude, double longitude)
    {
        await Clients.Group(requestId).SendAsync("TrackLocation", requestId, ambulanceId, latitude, longitude);
    }

    public async Task AmbulanceTripLocation(string clientId)
    {
        await Clients.Others.SendAsync("AmbulanceTripLocation", clientId);
    }

    public async Task SubscribeToLocation(string requestId)
    {
        // Add the connecting client to a group based on requestId
        await Groups.AddToGroupAsync(Context.ConnectionId, requestId);
    }

    public async Task UnsubscribeFromLocation(string requestId)
    {
        // Remove the client from the requestId group
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, requestId);
    }
}
