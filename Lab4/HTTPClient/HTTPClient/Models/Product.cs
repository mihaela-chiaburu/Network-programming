using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTPClient.Models
{
    public class Product
    {
        public int Id { get; set; }

        [JsonProperty("title")] 
        public string Name { get; set; }

        public decimal Price { get; set; }
        public int CategoryId { get; set; }
    }
}
