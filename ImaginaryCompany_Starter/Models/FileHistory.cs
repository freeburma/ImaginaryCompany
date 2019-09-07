using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ImaginaryCompany_Starter.Models
{
    public class FileHistory
    {
        public int Id { get; set; }

        [DisplayName("File Name")]    
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }
}
