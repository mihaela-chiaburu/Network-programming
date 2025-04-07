### Networking Programming Assignments

This repository contains lab work for the **Network Programming** course, using **C#** and **.NET**.

---

### Assignments Overview

#### **TCP Chat Application (Assignment #1)**
[Link to Lab #1](https://github.com/mihaela-chiaburu/Network-programming/tree/master/Lab1)

**Objective:**
- Develop a chat application using **TCP**.
- Learn how to handle connections and communication over TCP.
- Understand how to create and manage a server that can handle multiple clients concurrently.
- Implement client-server communication for message exchange.

**Description:**
In this assignment, I'll create a **TCP-based chat application** consisting of two parts:
1. A **server application** that listens for incoming connections on a specified port.
2. A **client application** that connects to the server and sends messages.

The server will display the messages it receives and broadcast them to all connected clients. Each client can send and receive messages, while the server ensures all clients are updated with the latest messages.

---

#### **UDP Chat Application (Assignment #2)**
[Link to Lab #2](https://github.com/mihaela-chiaburu/Network-programming/tree/master/Lab2)

**Objective:**
- Implement a chat application utilizing **UDP**.
- Understand the differences in communication when using UDP compared to TCP.
- Learn how to handle both multicast and unicast UDP messages.
- Explore how to create private channels alongside a general chat channel.

**Description:**
For this assignment, I will create a **UDP-based chat application**. This application will allow:
1. **Multicast communication** for public (general) messages.
2. **Private unicast communication** between specific clients.

Participants will send and receive messages in both public and private channels. Messages sent to the general channel will be visible to all participants, while private messages are only visible to the intended recipient.

---

#### **DNS Client Application (Assignment #3)**
[Link to Lab #3](https://github.com/mihaela-chiaburu/Network-programming/tree/master/Lab3)

**Objective:**
- Create a client application that performs **DNS queries**.
- Learn about the domain name resolution process and how to query DNS servers.
- Implement a feature to switch between custom DNS servers.

**Description:**
In this assignment, I will build a **DNS client application** that:
1. Resolves **domain names to IP addresses** and vice versa.
2. Allows the user to **change the DNS server** used for querying domain names.
3. Displays appropriate error messages if DNS resolution fails or if an invalid DNS server is specified.

The application will support both forward and reverse DNS lookups and will enable the user to configure a custom DNS server for queries.

---

#### **Online Store HTTP Application (Assignment #4)**
[Link to Lab #4](https://github.com/mihaela-chiaburu/Network-programming/tree/master/Lab4/OnlineStore)

**Objective:**
- Understanding how to make http requests
- Understanding how to serialize objects
- Understanding how to use the http client

**Description:**
In this assignment, I will create a **web-based online store** application that enables users to:
1. **Manage categories** (add, edit, delete).
2. **View product details** and list products within a category.
3. **Upload images** for products.
4. Perform **CRUD operations** on categories and products, such as creating, updating, and deleting them.

The application will utilize **HTTP requests** to interact with a backend server and will save product and category information to a database. Users will be able to view a list of categories, see products under each category, and manage their data efficiently.


---

This repository provides practical assignments for learning network programming concepts and implementing solutions in C# and .NET. Each assignment includes objectives, descriptions, and evaluation criteria to help you track your progress and proficiency in network programming.
