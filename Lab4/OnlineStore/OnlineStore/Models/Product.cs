using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace OnlineStore.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }

        public int CategoryId { get; set; }
        [ValidateNever]
        public Category Category { get; set; }
        public string Description { get; set; }
    }

}
