using HTTPClient.Constants;
using HTTPClient.Interfaces;
using HTTPClient.Models;
using HTTPClient.Services;
using System;
using System.Windows;
using System.Windows.Controls;

namespace HTTPClient
{
    public partial class MainWindow : Window
    {
        private ICategoryService _categoryService;
        private IProductService _productService;
        private Category _selectedCategory;

        public MainWindow()
        {
            InitializeComponent();
            InitializeServices();
            LoadCategories(null, null);
        }

        private void InitializeServices()
        {
            var httpService = new HttpService();
            _categoryService = new CategoryService(httpService);
            _productService = new ProductService(httpService);
        }

        // ---- CATEGORY ----
        private async void LoadCategories(object sender, RoutedEventArgs e)
        {
            try
            {
                var categories = await _categoryService.GetCategoriesAsync();
                CategoryListBox.ItemsSource = categories;
            }
            catch (Exception ex)
            {
                ShowErrorMessage(string.Format(AppConstants.Messages.ERROR_LOADING_CATEGORIES, ex.Message));
            }
        }

        private async void CategoryListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedCategory = CategoryListBox.SelectedItem as Category;
            if (_selectedCategory != null)
            {
                SelectedCategoryText.Text = $"Products in category: {_selectedCategory.Title}";
                CategoryTitleTextBox.Text = _selectedCategory.Title;
                await LoadProductsForSelectedCategory();
            }
        }

        private async Task LoadProductsForSelectedCategory()
        {
            if (_selectedCategory == null) return;

            try
            {
                var products = await _productService.GetProductsAsync(_selectedCategory.Id);
                ProductDataGrid.ItemsSource = products;
            }
            catch (Exception ex)
            {
                ShowErrorMessage(string.Format(AppConstants.Messages.ERROR_LOADING_PRODUCTS, ex.Message));
            }
        }

        private async void SearchCategory_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = SearchCategoryTextBox.Text;
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                ShowWarningMessage(AppConstants.Messages.ENTER_CATEGORY_NAME);
                return;
            }

            try
            {
                int categoryId = await _categoryService.GetCategoryIdByNameAsync(searchTerm);
                if (categoryId > 0)
                {
                    CategoryDetailsText.Text = $"Category ID: {categoryId}";
                    SelectCategoryInList(categoryId);
                }
                else
                {
                    CategoryDetailsText.Text = AppConstants.Messages.CATEGORY_NOT_FOUND;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(string.Format(AppConstants.Messages.ERROR_SEARCHING_CATEGORY, ex.Message));
            }
        }

        private async void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            string title = CategoryTitleTextBox.Text;
            if (string.IsNullOrWhiteSpace(title))
            {
                ShowWarningMessage(AppConstants.Messages.ENTER_CATEGORY_TITLE);
                return;
            }

            try
            {
                await _categoryService.CreateCategoryAsync(title);
                CategoryTitleTextBox.Clear();
                LoadCategories(null, null);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(string.Format(AppConstants.Messages.ERROR_ADDING_CATEGORY, ex.Message));
            }
        }

        private async void EditCategory_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedCategory == null)
            {
                ShowWarningMessage(AppConstants.Messages.SELECT_CATEGORY_FIRST);
                return;
            }

            string newTitle = CategoryTitleTextBox.Text;
            if (string.IsNullOrWhiteSpace(newTitle))
            {
                ShowWarningMessage(AppConstants.Messages.ENTER_NEW_TITLE);
                return;
            }

            try
            {
                await _categoryService.EditCategoryAsync(_selectedCategory.Id, newTitle);
                LoadCategories(null, null);
                CategoryTitleTextBox.Clear();
            }
            catch (Exception ex)
            {
                ShowErrorMessage(string.Format(AppConstants.Messages.ERROR_EDITING_CATEGORY, ex.Message));
            }
        }

        private async void DeleteCategory_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedCategory == null)
            {
                ShowWarningMessage(AppConstants.Messages.SELECT_CATEGORY_FIRST);
                return;
            }

            var result = MessageBox.Show(
                string.Format(AppConstants.Messages.DELETE_CONFIRMATION, _selectedCategory.Title),
                "Confirm",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes) return;

            try
            {
                await _categoryService.DeleteCategoryAsync(_selectedCategory.Id);
                LoadCategories(null, null);
                ClearProductData();
            }
            catch (Exception ex)
            {
                ShowErrorMessage(string.Format(AppConstants.Messages.ERROR_DELETING_CATEGORY, ex.Message));
            }
        }

        // ---- PRODUCT ----
        private async void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedCategory == null)
            {
                ShowWarningMessage(AppConstants.Messages.SELECT_CATEGORY_FIRST);
                return;
            }

            string name = ProductNameTextBox.Text;
            if (string.IsNullOrWhiteSpace(name))
            {
                ShowWarningMessage(AppConstants.Messages.ENTER_PRODUCT_NAME);
                return;
            }

            if (!decimal.TryParse(ProductPriceTextBox.Text, out decimal price) || price <= 0)
            {
                ShowWarningMessage(AppConstants.Messages.ENTER_VALID_PRICE);
                return;
            }

            try
            {
                await _productService.CreateProductAsync(_selectedCategory.Id, name, price);
                ClearProductInputs();
                await LoadProductsForSelectedCategory();
            }
            catch (Exception ex)
            {
                ShowErrorMessage(string.Format(AppConstants.Messages.ERROR_ADDING_PRODUCT, ex.Message));
            }
        }

        private void SelectCategoryInList(int categoryId)
        {
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

        private void ClearProductData()
        {
            ProductDataGrid.ItemsSource = null;
            SelectedCategoryText.Text = "";
            CategoryTitleTextBox.Clear();
        }

        private void ClearProductInputs()
        {
            ProductNameTextBox.Clear();
            ProductPriceTextBox.Clear();
        }

        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ShowWarningMessage(string message)
        {
            MessageBox.Show(message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}