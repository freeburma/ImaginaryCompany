using ImaginaryCompany.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImaginaryCompany.Data
{
    public static class SeedData
    {
        /// <summary>
        /// Seeding data
        /// Ref: https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/working-with-sql?view=aspnetcore-2.2&tabs=visual-studio
        /// </summary>
        /// <param name="serviceProvider"></param>
        public static void Initialize (IServiceProvider serviceProvider)
        {
            using (var context = new CompanyContext     (serviceProvider.GetRequiredService<DbContextOptions<CompanyContext>>()))
            {
                if (!context.Categories.Any())
                {
                    context.Categories.AddRange
                    (
                            new Category { CategoryName = "Coffee" },
                            new Category { CategoryName = "Sea Food" },
                            new Category { CategoryName = "Electronic" }
                    );

                    context.SaveChanges();
                }

                if (!context.Products.Any())
                {
                    context.Products.AddRange(
                          new Product { ProductName = "Snapper", Quantity = 44, Price = 15.99M, Category = context.Categories.Where( c => c.CategoryName == "Coffee").FirstOrDefault() },
                          new Product { ProductName = "iPhone 11", Quantity = 10, Price = 1024.99M, Category = context.Categories.Where( c => c.CategoryName == "Electronic").FirstOrDefault() },
                          new Product { ProductName = "Salmon", Quantity = 100, Price = 9.99M, Category = context.Categories.Where( c => c.CategoryName == "Sea Food").FirstOrDefault() },
                          new Product { ProductName = "Nestle Instance Coffee", Quantity = 30, Price = 3.99M, Category = context.Categories.Where( c => c.CategoryName == "Coffee").FirstOrDefault() }

                      );

                    context.SaveChanges();
                }

            }// end using{}

        }// end Initialize()
    }
}

