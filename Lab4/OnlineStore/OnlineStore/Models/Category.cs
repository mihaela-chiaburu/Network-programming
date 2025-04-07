using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace OnlineStore.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Title { get; set; }
        [ValidateNever]
        public ICollection<Product> Products { get; set; }
    }

}
