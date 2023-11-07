using Microsoft.AspNetCore.SignalR;

namespace AmbulanceLocationEmulator;

//public class AmbulanceLocationHub : Hub
//{
//    public async Task SendLocation(string ambulanceId, double latitude, double longitude)
//    {
//        // Broadcast the location update to all connected clients except the sender
//        await Clients.Others.SendAsync("ReceiveLocation", ambulanceId, latitude, longitude);
//    }
//}
