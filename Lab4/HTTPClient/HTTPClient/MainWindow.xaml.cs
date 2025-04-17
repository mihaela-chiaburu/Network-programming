using HTTPClient.Models;
using HTTPClient.Services;
using System;
using System.Windows;
using System.Windows.Controls;

namespace HTTPClient
{
    public partial class MainWindow : Window
    {
        private readonly ApiService _apiService;
        private Category _selectedCategory;

        public MainWindow()
        {
            InitializeComponent();
            _apiService = new ApiService();
            LoadCategories(null, null);
        }

        // Încărcare categorii (GET /api/Category/categories)
        private async void LoadCategories(object sender, RoutedEventArgs e)
        {
            try
            {
                var categories = await _apiService.GetCategoriesAsync();
                CategoryListBox.ItemsSource = categories;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading categories: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Când selectăm o categorie (GET /api/Category/categories/{id}/products)
        private async void CategoryListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedCategory = CategoryListBox.SelectedItem as Category;
            if (_selectedCategory != null)
            {
                SelectedCategoryText.Text = $"Products in category: {_selectedCategory.Title}";
                CategoryTitleTextBox.Text = _selectedCategory.Title; // Pre-fill edit box
                await LoadProductsForSelectedCategory();
            }
        }

        private async Task LoadProductsForSelectedCategory()
        {
            if (_selectedCategory == null) return;

            try
            {
                var products = await _apiService.GetProductsAsync(_selectedCategory.Id);
                ProductDataGrid.ItemsSource = products;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading products: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Căutare categorie după nume (GET /api/Category/categories/search)
        private async void SearchCategory_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = SearchCategoryTextBox.Text;
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                MessageBox.Show("Please enter a category name to search", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                int categoryId = await _apiService.GetCategoryIdByNameAsync(searchTerm);
                if (categoryId > 0)
                {
                    CategoryDetailsText.Text = $"Category ID: {categoryId}";

                    // Optionally select the category in the list
                    var categories = CategoryListBox.ItemsSource as IEnumerable<Category>;
                    if (categories != null)
                    {
                        var foundCategory = categories.FirstOrDefault(c => c.Id == categoryId);
                        if (foundCategory != null)
                        {
                            CategoryListBox.SelectedItem = foundCategory;
                        }
                    }
                }
                else
                {
                    CategoryDetailsText.Text = "Category not found";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching category: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Adăugare categorie (POST /api/Category/categories)
        private async void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            string title = CategoryTitleTextBox.Text;
            if (string.IsNullOrWhiteSpace(title))
            {
                MessageBox.Show("Please enter a category title", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                await _apiService.CreateCategoryAsync(title);
                CategoryTitleTextBox.Clear();
                LoadCategories(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding category: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Editare categorie (PUT /api/Category/{id})
        private async void EditCategory_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedCategory == null)
            {
                MessageBox.Show("Please select a category first", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string newTitle = CategoryTitleTextBox.Text;
            if (string.IsNullOrWhiteSpace(newTitle))
            {
                MessageBox.Show("Please enter a new title", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                await _apiService.EditCategoryAsync(_selectedCategory.Id, newTitle);
                LoadCategories(null, null);
                CategoryTitleTextBox.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error editing category: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Ștergere categorie (DELETE /api/Category/categories/{id})
        private async void DeleteCategory_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedCategory == null)
            {
                MessageBox.Show("Please select a category first", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Delete category '{_selectedCategory.Title}'?", "Confirm",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;

            try
            {
                await _apiService.DeleteCategoryAsync(_selectedCategory.Id);
                LoadCategories(null, null);
                ProductDataGrid.ItemsSource = null;
                SelectedCategoryText.Text = "";
                CategoryTitleTextBox.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting category: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Adăugare produs (POST /api/Category/categories/{id}/products)
        private async void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedCategory == null)
            {
                MessageBox.Show("Please select a category first", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string name = ProductNameTextBox.Text;
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Please enter a product name", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(ProductPriceTextBox.Text, out decimal price) || price <= 0)
            {
                MessageBox.Show("Please enter a valid price", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                await _apiService.CreateProductAsync(_selectedCategory.Id, name, price);
                ProductNameTextBox.Clear();
                ProductPriceTextBox.Clear();
                await LoadProductsForSelectedCategory();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding product: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}