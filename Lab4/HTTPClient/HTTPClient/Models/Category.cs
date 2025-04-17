using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTPClient.Models
{
    public class Category
    {
        public int Id { get; set; }

        [JsonProperty("name")] 
        public string Title { get; set; }

        public int ItemsCount { get; set; }
    }
}
