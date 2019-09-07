using ImaginaryCompany.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImaginaryCompany.Services
{
    /// <summary>
    /// Interface PDF service class 
    /// </summary>
    public interface I_PDFService
    {
        void GenerateProductTemplate (string filePath, List<Product> productList);
        void Test (string filePath);
        byte[] GetFile (string fileName);
    }
}
