using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ImaginaryCompany.Models;
using ImaginaryCompany.Data;
using ImaginaryCompany.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using ImaginaryCompany.Services;
using System.IO;

namespace ImaginaryCompany.Controllers
{
    public class CompanyController : Controller
    {
        private readonly CompanyContext _db;
        private readonly IHostingEnvironment _hostingEnvironment; /// Generating File on Server
        private readonly I_PDFService _pdf; /// PDF Sservice

        public CompanyController (CompanyContext db,
                            IHostingEnvironment hostingEnvironment,
                            I_PDFService pdf
                           )
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;
            _pdf = pdf;
        }

        #region Start Product 
        // ==================================================================================================
        // Start Product 
        // ==================================================================================================

        public IActionResult AddProduct ()
        {
            var productViewModel = new ProductViewModel();
            productViewModel.CategoryList = _db.Categories.ToList();

            return View(productViewModel);
        }// end AddProduct()


        [HttpPost]
        public IActionResult AddProduct (ProductViewModel model)
        {
            var product = new Product();
            product = model.Product;

            var category = _db.Categories.Find(model.CategoryId);
            product.Category = category;

            /// Adding the data to db
            _db.Products.Add(product);
            _db.SaveChanges();

            return RedirectToAction("ProductList");


        }// end AddCategory()


        public IActionResult EditProduct (int? id)
        {
            if (id == null) RedirectToAction("ProductList");

            /// Looking for the product
            var prod = _db.Products
                        .Where(p => p.ProductId == id)
                        .Include(c => c.Category)
                        .FirstOrDefault();

            if (prod == null) RedirectToAction("ProductList");

            /// Assigning to view model 
            var productViewModel = new ProductViewModel();
            productViewModel.CategoryId = prod.Category.CategoryId; /// Get the category id
            productViewModel.Product = prod; /// Getting the product 
            productViewModel.CategoryList = _db.Categories.ToList(); // Getting the category list 

            return View(productViewModel);
        }// end EditProduct()

        [HttpPost]
        public IActionResult EditProduct (int? id, ProductViewModel model)
        {
            if (id == null || !ModelState.IsValid) RedirectToAction("ProductList");

            var product = _db.Products.Find(id);
            if (product == null) RedirectToAction("ProductList");

            product.ProductName = model.Product.ProductName;
            product.Price = model.Product.Price;
            product.Quantity = model.Product.Quantity;

            var category = _db.Categories.Find(model.CategoryId);
            product.Category = category;


            /// Saving changes the data to db
            _db.SaveChanges();

            return RedirectToAction("ProductList");


        }// end EditProduct()

        public IActionResult DeleteProduct (int? id)
        {
            if (id == null) RedirectToAction("ProductList");

            /// Looking for the product
            var prod = _db.Products
                        .Where(p => p.ProductId == id)
                        .Include(c => c.Category)
                        .FirstOrDefault();

            if (prod == null) RedirectToAction("ProductList");

            /// Assigning to view model 
            var productViewModel = new ProductViewModel();
            productViewModel.CategoryId = prod.Category.CategoryId; /// Get the category id
            productViewModel.Product = prod; /// Getting the product 
            productViewModel.CategoryList = _db.Categories.ToList(); // Getting the category list 

            return View(productViewModel);
        }// end DeleteProduct()

        [HttpPost, ActionName("DeleteProduct")]
        public IActionResult DeleteProductConfirmed (int? id)
        {
            if (id == null) RedirectToAction("ProductList");

            var prodToDelete = _db.Products.Find(id);

            /// Removing the product form db 
            _db.Products.Remove(prodToDelete);
            _db.SaveChanges();

            return RedirectToAction("ProductList");

        }// end DeleteProduct()

        // ==================================================================================================
        // End Product 
        // ==================================================================================================
        #endregion

        #region Start Category

        // ==================================================================================================
        // Start Product 
        // ==================================================================================================

        public IActionResult AddCategory ()
        {
            return View();

        }// end AddCategory()

        [HttpPost]
        public IActionResult AddCategory (Category model)
        {
            _db.Categories.Add(model);
            _db.SaveChanges();

            return RedirectToAction("ProductList");

        }// end AddCategory()


        public IActionResult EditCategory (int? id)
        {
            if (id == null) return RedirectToAction("ProductList");

            var categoryToEdit = _db.Categories.Find(id);


            return View(categoryToEdit);

        }// end AddCategory()

        [HttpPost]
        public IActionResult EditCategory (int? id, Category model)
        {
            if (id == null) return RedirectToAction("ProductList");

            var categoryToEdit = _db.Categories.Find(id);
            categoryToEdit.CategoryName = model.CategoryName;

            _db.SaveChanges();


            return RedirectToAction("ProductList");

        }// end AddCategory()


        public IActionResult DeleteCategory (int? id)
        {
            if (id == null) return RedirectToAction("ProductList");

            var categoryToDelete = _db.Categories.Find(id);


            return View(categoryToDelete);

        }// end DeleteCategory()

        [HttpPost, ActionName("DeleteCategory")]
        public IActionResult DeleteCategoryConfirmed (int? id)
        {
            if (id == null) return RedirectToAction("ProductList");

            var categoryToDelete = _db.Categories.Find(id);


            /// Removing the category form db
            _db.Categories.Remove(categoryToDelete);
            _db.SaveChanges();

            return RedirectToAction("ProductList");

        }// end DeleteCategory()


        // ==================================================================================================
        // End Product 
        // ==================================================================================================
        #endregion

        #region Product List
        public IActionResult ProductList ()
        {
            var productViewModel = new ProductCategoryViewModel();
            productViewModel.ProductList = _db.Products.ToList();
            productViewModel.CategoryList = _db.Categories.ToList();

            return View(productViewModel);
        }// end ProductList()

        #endregion


        #region Generate PDF

        /// <summary>
        /// Generating the PDF files in "wwwroot/documents/" directory
        /// </summary>
        /// <returns></returns>
        public IActionResult GenerateProductPDF ()
        {

            var prodList = _db.Products.ToList();
            var fileName = $"Product-{DateTime.Now.ToFileTimeUtc()}.pdf";
            var fileBasePath = $"{_hostingEnvironment.WebRootPath}\\documents"; 
            var filePath = $"{fileBasePath}\\{fileName}";

            /// Checking the directory
            if (!Directory.Exists(fileBasePath))
            {
                Directory.CreateDirectory(fileBasePath);
            }

            _pdf.GenerateProductTemplate(filePath, prodList);
            // _pdf.Test(filePath);

            /// Preparing the instance to add to db 
            var fileHistoryToAdd = new FileHistory();
            fileHistoryToAdd.FileName = fileName;
            fileHistoryToAdd.FilePath = filePath;

            /// Adding to db
            _db.FileHistories.Add(fileHistoryToAdd);
            _db.SaveChanges();

            return RedirectToAction("ProductList");

        }// end GenerateProductPDF()

        /// <summary>
        /// Displaying PDF list
        /// </summary>
        /// <returns></returns>
        public IActionResult DocumentList()
        {
            return View(_db.FileHistories.ToList());
        }// end DownloadPDF()



        public IActionResult DownloadPDF(int? id)
        {
            if (id == null) RedirectToAction("DocumentList");

            var getPDF_Path = _db.FileHistories.Find(id); 
            if (getPDF_Path == null) RedirectToAction("DocumentList");

            /// Checking the file on disk
            if (System.IO.File.Exists(getPDF_Path.FilePath))
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(Url.Content($"{getPDF_Path.FilePath}"));

                return File(fileBytes, "application/pdf", getPDF_Path.FileName);
            }
            else
            {
                return RedirectToAction("DocumentList");
            }
        }// end DownloadPDF()

        #endregion


    }
}
