using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VertoTest.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VertoTest.Controllers
{
    public class ContentController : Controller
    {
        // GET: /<controller>/
        public IActionResult Api()
        {
            List<ContentModel> contents = new List<ContentModel>();
            
            return View("Api", contents);
        }
    }
}

