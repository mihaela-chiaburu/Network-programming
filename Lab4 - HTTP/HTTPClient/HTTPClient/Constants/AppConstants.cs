using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTPClient.Constants
{
    public static class AppConstants
    {
        public const string BASE_URL = "https://localhost:44370/api/Category/";
        public const string CONTENT_TYPE = "application/json";

        public static class Endpoints
        {
            public const string CATEGORIES = "categories";
            public const string CATEGORIES_SEARCH = "categories/search";
            public const string CATEGORY_BY_ID = "{0}";
            public const string CATEGORY_PRODUCTS = "categories/{0}/products";
            public const string DELETE_CATEGORY = "categories/{0}";
        }

        public static class Messages
        {
            public const string ENTER_CATEGORY_NAME = "Please enter a category name to search";
            public const string ENTER_CATEGORY_TITLE = "Please enter a category title";
            public const string ENTER_NEW_TITLE = "Please enter a new title";
            public const string ENTER_PRODUCT_NAME = "Please enter a product name";
            public const string ENTER_VALID_PRICE = "Please enter a valid price";
            public const string SELECT_CATEGORY_FIRST = "Please select a category first";
            public const string CATEGORY_NOT_FOUND = "Category not found";
            public const string DELETE_CONFIRMATION = "Delete category '{0}'?";

            public const string ERROR_LOADING_CATEGORIES = "Error loading categories: {0}";
            public const string ERROR_LOADING_PRODUCTS = "Error loading products: {0}";
            public const string ERROR_SEARCHING_CATEGORY = "Error searching category: {0}";
            public const string ERROR_ADDING_CATEGORY = "Error adding category: {0}";
            public const string ERROR_EDITING_CATEGORY = "Error editing category: {0}";
            public const string ERROR_DELETING_CATEGORY = "Error deleting category: {0}";
            public const string ERROR_ADDING_PRODUCT = "Error adding product: {0}";
        }
    }
}
