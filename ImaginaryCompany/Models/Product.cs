using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ImaginaryCompany.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [DisplayName("Product Name")]
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        /// Adding as a ForeignKey
        public Category Category { get; set; }
    }
}
