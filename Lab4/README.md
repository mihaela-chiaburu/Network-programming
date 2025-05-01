# Network Programming Lab 4 - HTTP Client Application

## Overview
This laboratory assignment involves creating a WPF (Windows Presentation Foundation) application that communicates with an HTTP API for an online shop. The solution consists of two projects:

- **UtmShop**: A simple API project provided by the professor, using SQLite to store pre-registered data for the online shop.
- **HTTPUser**: A WPF application that serves as the user interface, communicating with the API using a singleton HTTP client.

<p align="center">
    <kbd>
        <img src="https://github.com/user-attachments/assets/6a9abe3a-9179-4d29-b240-98ab600de2a4" alt="Interface of the app" style="border: 10px solid black; padding: 10px; max-width: 20%; height: 350px;">
    </kbd>
    <p align="center"><em>Interface of the app</em></p>
</p>

The application will implement basic CRUD operations for categories and products, allowing interaction with the online shop through an easy-to-use UI.

## Technologies Used

- **.NET Core / .NET Framework**
- **WPF (Windows Presentation Foundation)**
- **HttpClient**
- **SQLite (for the API)**

## Objectives

- Understand how to make HTTP requests.
- Learn how to serialize and deserialize objects.
- Learn how to utilize an HTTP client for API communication.

## Features

- **List categories**: Displays all categories in the shop.
- **Category details**: Shows details of a selected category.
- **Create categories**: Allows users to add new categories to the shop.
- **Delete categories**: Removes categories from the shop.
- **Update category title**: Modify the title of a category.
- **Add products**: Create new products in a selected category.
- **View products**: Display all products within a category.

## How to Use

1. **Clone the repository** and build the project in Visual Studio.
2. **Run the UtmShop API project** to ensure the online shop API is operational.
3. **Launch the HTTPUser WPF application** by accessing the exe from the `\HTTPClient\bin\Debug\net8.0-windows` folder and use the UI to interact with the API. The application allows you to perform CRUD operations on categories and products.


### Main Application UI
<p align="center">
    <kbd>
        <img src="https://github.com/user-attachments/assets/f94aa9c3-a53e-4664-9b01-b28b3de6d514" alt="Interface of the app" style="border: 10px solid black; padding: 10px; max-width: 20%; height: 350px;">
    </kbd>
    <p align="center"><em>Interface of the app</em></p>
</p>

---

## Evaluation Criteria:

- The application can list the categories – 1 point  
- The application can show details about a category – 1 point  
- The application can create a new category – 2 points  
- The application can delete a category – 1 point  
- The application can modify the title of a category – 2 points  
- The application can create new products in a category – 2 points  
- The application can view the list of products in a category – 1 point
