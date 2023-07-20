# Mirror-Events
Mirror-Events is a free add-on to the [Mirror](https://github.com/MirrorNetworking/Mirror) networking library for Unity created by Caleb Brock of Calroot Digital. It adds additional functionality to Mirror that allows developers to more easily create networked events to easily track game state changes.

### What does it do?
Mirror-Events adds an event-handling system to Mirror that allows you to easily invoke and observe NetworkEvents.

### How is it useful?
There's many cases where this could be useful, and it is particularly when many different systems rely on a single event being triggered.
For example; when a player joins your match and you need to notify your chat system that they've joined so a message can be sent, your scoreboard system so that they can add the player to your scoreboard, and your game mode system so that it can handle what it needs to, this is the perfect use case.
Other use cases where this can be helpful are those such as when writing a chat system, handling major game state changes (match started, ended, etc), or when a player dies or respawns.

## Installation
Download the latest .unitypackage from the releases page and import it into your project.

## How do I use it?
Simply! 

Firstly you just need to register the Server and Client as part of the NetworkEvent system inside of your NetworkManager:
```csharp 
// Register the server to handle network events in OnStartServer()
public override void OnStartServer() {
  base.OnStartServer();
  EventManager.Server_Register();
}

// Unregister the server from handling network events in OnStopServer()
public override void OnStopServer() {
  base.OnStopServer();
  EventManager.Server_Unregister();
}

// Register the client to handle network events in OnStartClient()
public override void OnStartClient() {
  base.OnStartClient();
  EventManager.Client_Register();
}

// Unregister the client from handling network events in OnStopClient()
public override void OnStopClient() {
  base.OnStopClient();
  EventManager.Client_Unregister();
}
```

Then you just need to create an event class that inherits from `NetworkEvent`:
```csharp
public class PlayerDeathEvent : NetworkEvent {
    public uint playerId;
    public uint killerId;
    public string weaponName;

    public override void Write(NetworkWriter writer) {
        // Make sure you call the base.Write() method
        base.Write(writer);
        // Make sure you write and read in the same order!
        writer.WriteUInt(playerId);
        writer.WriteUint(killerId);
        writer.WriteString(weaponName);
    }

    public override void Read(NetworkReader reader) {
        // Make sure you call the base.Read() method
        base.Read(reader);
        // Make sure you read in the same order you write!
        playerId = reader.ReadUInt();
        killerId = reader.ReadUInt();
        weaponName = reader.ReadString();
    }
}
```

Then, to invoke the event you can call any of the various methods on the `EventManager` class:
```csharp
[Server]
void Server_KillPlayer(uint killerId, string weaponName) {
    EventManager.Server_InvokeNetworkEvent(new PlayerDeathEvent {
        playerId = netId,
        killerId = killerId,
        weaponName = weaponName
    });
}
```

Make sure the class with the event handler will be registered to listen to the events (and unregisters when it is destroyed or otherwise no longer in use):
```csharp
public void Start() {
    EventManager.RegisterListeners(this);
}

void OnDestroy() {
    EventManager.UnregisterListeners(this);
}
```

And then create a method inside of the class with the `NetworkEventHandler` attribute and the `NetworkEvent`-inherited type as the parameter:
```csharp

[Server, NetworkEventHandler]
public void AddScore(PlayerDeathEvent e) {
    if (redTeam.Contains(e.killerId)) {
        redTeamScore++;
    } else {
        blueTeamScore++;
    }
}
```


## FAQ
### Will this work on non-NetworkBehaviour-derived classes?
Yes. This will work from any C# class so long as it is registered with `EventHandler.RegisterEvents()`

### Do event handlers respect \[Server\] and \[Client\] tags?
Yes!! If you have registered a class on both the client and a server specifying a \[Server\] or \[Client\] attribute will ensure the NetworkEventHandler will only listen on that 'side'.

### I have a question or am confused about something, where can I reach out to you? 
You can contact me on discord (rhetorical) and I can try to answer any questions.
