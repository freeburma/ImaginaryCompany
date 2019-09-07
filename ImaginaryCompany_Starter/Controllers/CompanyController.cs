using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ImaginaryCompany_Starter.Models;
using ImaginaryCompany_Starter.Data;
using ImaginaryCompany_Starter.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace ImaginaryCompany_Starter.Controllers
{
    public class CompanyController : Controller
    {
        private readonly CompanyContext _db;
        private readonly IHostingEnvironment _hostingEnvironment; /// Generating File on Server

        public CompanyController (CompanyContext db,
                            IHostingEnvironment hostingEnvironment
                           )
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;
        }
        

    }// end class{}
}
