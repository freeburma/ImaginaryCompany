using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ImaginaryCompany_Starter.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [DisplayName("Category Name")]    
        public string CategoryName { get; set; }
    }
}
