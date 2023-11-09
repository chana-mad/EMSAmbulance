const connection = new signalR.HubConnectionBuilder()
    .withUrl("/locationHub")
    .configureLogging(signalR.LogLevel.Information)
    .build();

connection.on("ReceiveLocation", (ambulanceId, latitude, longitude) => {
    console.log(`${ambulanceId} - Location: ${latitude}, ${longitude}`);
    // Update the location on the map or user interface
});
let count = 0;
connection.on("TrackLocation", (requestId, ambulanceId, latitude, longitude) => {
    console.log(`${ambulanceId} - Location: ${latitude}, ${longitude}, ${requestId}`);
    var all = [
        
        { lat: 52.111689, lng: 4.282528 },
        { lat: 52.111352, lng: 4.282828 },
        { lat: 52.11104, lng: 4.282442 },
        { lat: 52.110663, lng: 4.281876 },
        { lat: 52.110264, lng: 4.281363 },//
        { lat: 52.110727, lng: 4.280537 },
        { lat: 52.111029, lng: 4.280468 },//
        { lat: 52.111383, lng: 4.281034 },
        { lat: 52.111684, lng: 4.281506 },
        { lat: 52.111982, lng: 4.281752 },
        { lat: 52.112198, lng: 4.28142 },
        { lat: 52.112457, lng: 4.281374 },
        { lat: 52.112809, lng: 4.281943 },
        { lat: 52.113076, lng: 4.282326 },
        { lat: 52.113306, lng: 4.282568 }
    ];

    let dest;
    
    if (count == 14) {
        count = 0;
    } 

    dest = all[count];
    count = count + 1;

    console.log(count);
    console.log(dest);

    calculateAndDisplayRoute(dest)
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

