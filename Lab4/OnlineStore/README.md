# Online Store HTTP Application

This is an online store web application built using .NET MVC, which allows users to manage product categories and products. Users can perform CRUD (Create, Read, Update, Delete) operations for categories and products through an easy-to-use web interface. The application interacts with a database and provides functionality to display categories, manage products, and more.

<p align="center">
    <kbd>
        <img src="https://github.com/user-attachments/assets/9870c627-c9b3-449c-8c41-b0c7d4a34ca1" alt="Interface of the app" style="border: 10px solid black; padding: 10px; max-width: 50%; height: 350px;">
    </kbd>
    <p align="center"><em>Interface of the app</em></p>
</p>

## Requirements

- Windows operating system
- .NET 8.0 or later runtime (if running from source)
- A modern web browser (Chrome, Firefox, Edge)

## How to Run

1. Download the files from the repository (either by cloning or downloading the ZIP).
2. Open the project in Visual Studio and build the solution.
3. Run the application by pressing `F5` in Visual Studio.
4. Once the application is running, navigate to the default URL.
5. The application will display a list of products cards

### App interface

<p align="center">
    <kbd>
        <img src="https://github.com/user-attachments/assets/1f5b3014-9e86-485f-acac-ce8a605e1090" alt="List of Products" style="border: 10px solid black; padding: 10px; max-width: 50%; height: 250px;">
    </kbd>
    <p align="center"><em>List of Products</em></p>
</p>

<p align="center">
    <kbd>
        <img src="https://github.com/user-attachments/assets/4ce1207b-94db-4d4e-8787-108f7379ad73" alt="All Categories" style="border: 10px solid black; padding: 10px; max-width: 50%; height: 200px;">
    </kbd>
    <p align="center"><em>List of Categoris</em></p>
</p>

### Description

This web application allows users to manage categories and products in an online store. It includes the following features:

- **Category Management:**  
   - Users can view, create, edit, and delete categories.
   - Each category can contain a list of products.
   - Categories can be associated with multiple products, and users can manage these associations easily.

- **Product Management:**  
   - Users can create products within a specific category.
   - Users can view the list of products in a category and perform CRUD operations.

### Assignment Description  

**Purpose:**  
The application is designed to demonstrate the use of HTTP requests (GET, POST, PUT, DELETE) for managing categories and products in an online store. It also shows how to serialize objects for communication between the client and the server.

### Functionality

1. **List Categories (GET)**  
   The application fetches and displays a list of categories from the server.

2. **View Category Details (GET)**  
   Clicking on a category will show detailed information about that category and its products.
   
<p align="center">
    <kbd>
        <img src="https://github.com/user-attachments/assets/093b5eaa-183d-4041-8fb3-34e7ee49f663" alt="Category Details" style="border: 10px solid black; padding: 10px; max-width: 50%; height: 200px;">
    </kbd>
    <p align="center"><em>Category Details</em></p>
</p>

4. **Create a New Category (POST)**  
   Users can add a new category by submitting a form. The application will save the category to the database.
   
<p align="center">
    <kbd>
        <img src="https://github.com/user-attachments/assets/5af4abb0-8485-4ea4-ac4e-70a5119aac09" alt="Create a New Category" style="border: 10px solid black; padding: 10px; max-width: 50%; height: 200px;">
    </kbd>
    <p align="center"><em>Create a New Category</em></p>
</p>

5. **Edit an Existing Category (PUT)**  
   Users can edit the title of an existing category.

<p align="center">
    <kbd>
        <img src="https://github.com/user-attachments/assets/604e381f-dbe0-4f90-9f3a-9a45bdbbb715" alt="Edit an Existing Category" style="border: 10px solid black; padding: 10px; max-width: 50%; height: 200px;">
    </kbd>
    <p align="center"><em>Edit an Existing Category</em></p>
</p>

6. **Delete a Category (DELETE)**  
   Users can delete a category from the list.

<p align="center">
    <kbd>
        <img src="https://github.com/user-attachments/assets/46dcb0da-1680-4ca9-bdf9-fdf2dc235ae3" alt="Delete a Category" style="border: 10px solid black; padding: 10px; max-width: 50%; height: 150px;">
    </kbd>
    <p align="center"><em>Delete a Category</em></p>
</p>

7. **Create a Product in a Category (POST)**  
   Users can add a product to a category by providing product details like name, price, and category selection.

<p align="center">
    <kbd>
        <img src="https://github.com/user-attachments/assets/2ad0217c-8a84-4c46-a2b0-6a84d884258a" alt="Create a Product" style="border: 10px solid black; padding: 10px; max-width: 50%; height: 350px;">
    </kbd>
    <p align="center"><em>Create a Product</em></p>
</p>

8. **List Products in a Category (GET)**  
   For each category, users can view the products that belong to that category.

### Evaluation Criteria

1. **The application successfully lists all categories** – **1 point**
2. **The application displays detailed information about a category** – **1 point**
3. **The application allows the creation of a new category** – **2 points**
4. **The application allows the deletion of a category** – **1 point**
5. **The application allows updating the title of a category** – **2 points**
6. **The application allows the creation of new products within a category** – **2 points**
7. **The application can list products within a category** – **1 point**

### Conclusion

This project demonstrates how to build a simple yet functional online store application using .NET MVC. It covers basic CRUD operations for both categories and products and utilizes HTTP requests to handle data manipulation and display. The application provides an intuitive interface for managing categories and products in an online store environment.
