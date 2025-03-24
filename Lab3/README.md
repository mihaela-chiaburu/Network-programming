# DNS Client Application

This is a DNS client application built with C# that allows users to resolve domain names to IP addresses and vice versa, and to configure a custom DNS server for queries. The application can resolve domain names, check the IP addresses associated with a given domain, and use a custom DNS server for making DNS queries.

## Requirements

- Windows operating system
- .NET 8.0 or later runtime (if running from source)

## How to Run

1. Download the files from the repository (either by cloning or downloading the ZIP).
2. Open the project in Visual Studio and build the solution.
3. Run the application by pressing `F5` or by double-clicking the executable file (`DNS_Client.exe`).
4. In the application, you can enter a domain name or IP address in the "Resolve" input box and click the "Resolve" button to resolve it.
5. To use a custom DNS server, enter the DNS server address in the "DNS Server" input box and click the "Use DNS" button.

## Features

- **Domain Resolution**: Resolve a domain name to its associated IP address or vice versa.
- **Custom DNS Server**: Change the DNS server used for resolving domain names by entering a custom DNS server address.
- **DNS Querying**: Uses both the default DNS server and the specified custom DNS server to resolve queries.
- **Error Handling**: Displays relevant error messages if DNS resolution fails or if the DNS server is unreachable.
  
## Notes

- The application uses DNS resolution and allows switching between default and custom DNS servers.
- The "Resolve" button resolves domain names or IP addresses, showing either the associated IPs or domains.
- The "Use DNS" button allows you to specify a custom DNS server for the resolution process.
- The application supports both **forward DNS resolution** (domain to IP) and **reverse DNS resolution** (IP to domain).
- Ensure your custom DNS server is reachable to avoid resolution errors.
