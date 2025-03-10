# Network Programming Labs

This repository contains labs for understanding network programming. Below are the descriptions and links to the labs:

## Lab #1: Chat Application Using TCP
[Link to Lab #1](https://github.com/mihaela-chiaburu/Network-programming/tree/master/Lab1)

### Objective:
- Understand how to create a socket.
- Understand how a TCP server starts listening for connections on a port.
- Understand how to receive data through a socket.
- Understand how to send data through a socket.
- Learn how to handle multiple clients simultaneously.
- Learn how to properly close a connection from both the client and server side.

### Description:
In this lab, you need to create two console applications:
1. A server application that listens for incoming connections on a specific port.
2. A client application that connects to the server and sends messages.

The client application will ask the user to input a text message, which will then be sent to the server. The server will display this message and broadcast it to all connected clients, including the one who sent the message.
Clients can send multiple messages, and the server will handle them and display them on all connected clients' screens.

### Evaluation Criteria:
- The server and client are created and can connect to the server (5 points).
- The server can accept multiple clients concurrently (1 point).
- The client can send messages to the server, and the server displays the message in its window (1 point).
- The server can broadcast messages to all clients (1 point).
- Clients are able to display received messages (1 point).
- Proper connection, disconnection, and data transmission without critical exceptions (1 point).

---

## Lab #2: Chat Application Using UDP
[Link to Lab #2](https://github.com/mihaela-chiaburu/Network-programming/tree/master/Lab2)

### Objective:
- Understand how to create a UDP socket.
- Understand how to send UDP messages.
- Understand how to receive UDP messages.
- Understand how to send broadcast messages.

### Description:
In this lab, you need to create a chat application that works within a network segment. The chat will have a general channel where multicast messages will be received and displayed to all participants. Additionally, the application will allow private conversations between participants, which will not be displayed in the general channel.
Clients can send and receive both public (general channel) and private messages.

### Evaluation Criteria:
- A UDP socket capable of transmitting and receiving messages is created (1 point).
- The client can send messages to a specific IP address (2 points).
- The client can receive messages from a specific IP address (2 points).
- The client can send messages to the general channel (2 points).
- The client can receive and display messages from the general channel (2 points).
- Exceptions in data transmission are handled properly (1 point).

---

This README provides an overview of the network programming labs, highlighting the objectives and evaluation criteria for each lab. You can visit the provided links to access the full labs and their source code.
