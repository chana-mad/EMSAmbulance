const connection = new signalR.HubConnectionBuilder()
    .withUrl("/locationHub")
    .configureLogging(signalR.LogLevel.Information)
    .build();

connection.on("ReceiveLocation", (ambulanceId, latitude, longitude) => {
    console.log(`${ambulanceId} - Location: ${latitude}, ${longitude}`);
    // Update the location on the map or user interface
});

connection.on("TrackLocation", (requestId, ambulanceId, latitude, longitude) => {
    console.log(`${ambulanceId} - Location: ${latitude}, ${longitude}, ${requestId}`);
    // Update the location on the map or user interface
});

async function start() {
    try {
        await connection.start();
        subscribeToLocation('88889999-009');
        console.log("SignalR Connected.");
    } catch (err) {
        console.log(err);
        setTimeout(start, 5000);
    }
};

function subscribeToLocation(requestId) {
    connection.invoke("SubscribeToLocation", requestId)
        .catch(function (err) {
        return console.error(err.toString());
    });
}

// To unsubscribe if the user no longer wishes to receive updates
function unsubscribeFromLocation(requestId) {
    connection.invoke("UnsubscribeFromLocation", requestId).catch(function (err) {
        return console.error(err.toString());
    });
}

connection.onclose(async () => {
    await start();
});

// Start the connection.
start();

