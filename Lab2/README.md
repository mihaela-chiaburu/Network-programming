# UDP Chat Server/Client Program

This is a chat server/client application built with C# that allows clients to connect and send messages to each other.

## Requirements

- Windows operating system
- .NET 8.0 or later runtime (if running from source)

## How to Run

1. Download the files from the repository (either by cloning or downloading the ZIP).
2. In the `Server` folder, navigate to `bin/Debug/net8.0-windows`.
3. Double-click on the `Server.exe` file to start the server.
4. The server will automatically start listening for incoming connections.
5. In the `Client` folder, navigate to `bin/Debug/net8.0-windows`, and double-click on the `Client.exe` file to start the client.

## Notes

- Make sure the server is allowed through your firewall on port 65432.
- The client will attempt to connect to the server at the default address and port (localhost:65432).
