using ImaginaryCompany.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImaginaryCompany.ViewModels
{
    public class ProductViewModel
    {
        public int CategoryId { get; set; }
        public Product Product { get; set; }

        public List<Category> CategoryList { get; set; }
    }
}
