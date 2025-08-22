using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTPClient.Models
{
    public class CreateProductRequest
    {
        public string title { get; set; }
        public decimal price { get; set; }
    }
}
