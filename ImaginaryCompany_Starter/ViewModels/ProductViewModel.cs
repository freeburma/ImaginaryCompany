using ImaginaryCompany_Starter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImaginaryCompany_Starter.ViewModels
{
    public class ProductViewModel
    {
        public int CategoryId { get; set; }
        public Product Product { get; set; }

        public List<Category> CategoryList { get; set; }
    }
}
