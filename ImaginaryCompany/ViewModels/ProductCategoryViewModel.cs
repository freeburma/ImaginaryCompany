using ImaginaryCompany.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImaginaryCompany.ViewModels
{
    public class ProductCategoryViewModel 
    {

        public List<Product> ProductList { get; set; }

        public List<Category> CategoryList { get; set; }
    }
}
