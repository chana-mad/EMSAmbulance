// This is a dummy console application to simulate the ambulance sending its location.
using Microsoft.AspNetCore.SignalR.Client;
using System;

var connection = new HubConnectionBuilder()
    .WithUrl("https://localhost:7269/locationHub")
    .WithAutomaticReconnect()
    .Build();

try
{
    connection.On<string>("AmbulanceTripLocation", (message) =>
    {
        Console.WriteLine($"Recieved message from {message}");
    });

    await connection.StartAsync();
    Console.WriteLine("Connected to Hub");
}
catch(Exception e)
{
    Console.WriteLine($"Error connecting to server : {e.Message}");
}


// Simulating sending location updates
var random = new Random();
while (true)
{
    try
    {
        string requestId = "88889999-009";
        string ambulanceId = "Ambulance123";
        double latitude = 37.773972 + random.NextDouble() * 0.001; // Simulated latitude
        double longitude = -122.431297 + random.NextDouble() * 0.001; // Simulated longitude

        // Send the location update to the hub
        await connection.InvokeAsync("SendLocation", ambulanceId, latitude, longitude);
        await connection.InvokeAsync("TrackLocation", requestId, ambulanceId, latitude, longitude);

        Console.WriteLine($"Sent location update: {latitude}, {longitude}");

        // Wait for 1 second before sending the next update
        await Task.Delay(10000);
    }
    catch(Exception ex)
    {
        Console.WriteLine($"Error sending location update: {ex.Message}");
    }
}
