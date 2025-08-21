# UDP Chat Program

This is a UDP chat application built with C# that allows multiple clients to connect and send messages to each other. There is no server component in this application; all clients communicate directly with each other using UDP.

<img width="1919" height="1015" alt="Screenshot 2025-08-21 230952" src="https://github.com/user-attachments/assets/0d4c3471-62a3-4f05-914e-fbb2da8d2eca" />


## Requirements

- Windows operating system
- .NET 8.0 or later runtime (if running from source)

## How to Run

1. Download the files from the repository (either by cloning or downloading the ZIP).
2. In the `Client` folder, navigate to `bin/Debug/net8.0-windows`.
3. Double-click on the `Client.exe` file to start the client.
4. To open multiple clients, simply repeat the process on different windows or devices by double-clicking `Client.exe` in the `bin/Debug/net8.0-windows` folder.
5. Each client will automatically generate a random username and IP address.
6. Once all clients are running, they can communicate with each other by sending public or private messages.

## Notes

- The application uses UDP for communication, and messages are sent as datagrams to other clients in the network.
- Each client will automatically try to send and receive messages on port 9000.
- Ensure that UDP traffic is allowed through your firewall on port 9000 to ensure communication between clients.
- You do not need a server to run this application; multiple clients will interact directly using UDP multicast and unicast communication.

