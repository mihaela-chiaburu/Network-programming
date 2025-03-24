# DNS Client Application

This is a DNS client application built with C# that allows users to resolve domain names to IP addresses and vice versa, and to configure a custom DNS server for queries. The application can resolve domain names, check the IP addresses associated with a given domain, and use a custom DNS server for making DNS queries.

<p align="center">
    <kbd>
        <img src="https://github.com/user-attachments/assets/e533aa75-0beb-4d82-8843-695069cf583d" alt="Interface of the app" style="border: 10px solid black; padding: 10px; max-width: 20%; height: 350px;">
    </kbd>
    <p align="center"><em>Interface of the app</em></p>
</p>

## Requirements

- Windows operating system
- .NET 8.0 or later runtime (if running from source)

## How to Run

1. Download the files from the repository (either by cloning or downloading the ZIP).
2. Open the project in Visual Studio and build the solution.
3. Run the application by pressing `F5` or by double-clicking the executable file (`DNS_Client.exe`) by navigating to `DNS_Client/bin/Debug/net8.0-windows`.
4. In the application, you can enter a domain name or IP address in the "Resolve" input box and click the "Resolve" button to resolve it.
5. To use a custom DNS server, enter the DNS server address in the "DNS Server" input box and click the "Use DNS" button.

### Assignment Description  

**Purpose:** To create a console application that allows the user to:

- Resolve domain names to IP addresses or vice versa.
- Change the DNS server used for resolving domain names to a custom DNS server.

**Requirements:**

1. **Command: `resolve <domain>` or `resolve <ip>`**  
   This command will display the list of IP addresses assigned to the domain or the list of domains assigned to the given IP address. The system will use the default DNS server set by the system until the user specifies a custom DNS server.

2. **Command: `use dns <ip>`**  
   This command will allow the user to change the DNS server to the specified IP address for subsequent resolve queries.

**Evaluation Criteria:**

- The application can successfully resolve IP addresses from domain names. **(3 points)**
- The application can successfully resolve domains from IP addresses. **(3 points)**
- The application can switch to a custom DNS server for queries. **(2 points)**
- The application correctly handles errors when the DNS server cannot resolve a query. **(1 point)**
- The application displays an appropriate error message when an invalid DNS server address is provided. **(1 point)**
