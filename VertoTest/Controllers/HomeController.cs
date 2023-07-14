using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using VertoTest.Models;

namespace VertoTest.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly IWebHostEnvironment _environment;

    public HomeController(ILogger<HomeController> logger, IWebHostEnvironment environment)
    {
        _logger = logger;
        _environment = environment;
    }

    
    public IActionResult Index()
    {

        List<ContentModel> contents = new List<ContentModel>();

        using (var db = new ContentContext())
        {
            contents = db.Content.ToList();
        }

        ViewBag.contents = contents;

        
        return View();
    }

    
    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Help()
    {
        return View();
    }

    public IActionResult MyOpticron()
    {
        return View();
    }

    public IActionResult NewsAndReviews()
    {
        return View();
    }

    public IActionResult OurProducts()
    {
        return View();
    }

    public IActionResult WhereToBuy()
    {
        return View();
    }


    public IActionResult Edit()
    {

        return View();
    }

    [HttpPost]
    public IActionResult Edit(ContentModel entry)
    {
        using (var db = new ContentContext())
        {

            db.Add(entry);
            db.SaveChanges();
        }
        return View("Index");
    }




    public IActionResult AdminView()
    {
        List<ContentModel> contents = new List<ContentModel>();

        using(var db = new ContentContext())
        {
            contents = db.Content.ToList();
        }

        ViewBag.contents = contents;
        
        return View();
    }

    [HttpPost]
    public IActionResult AdminView(ContentModel entry)
    {
        using (var db = new ContentContext())
        {

            db.Add(entry);
            db.SaveChanges();
        }
        return View();
    }


    
    [HttpPost]
    public async Task<IActionResult> SubmitChanges(string text, int id, string text2, IFormFile file)
    {
        

        if (file != null && file.Length > 0)
        {
            // Generate a unique file name
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;



            // Get the file path for the static folder
            string filePath = Path.Combine(_environment.WebRootPath, "static", uniqueFileName);

            // Save the file to the static folder
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
               await file.CopyToAsync(stream);
            }

            string relativeFilePath = Path.Combine("/../static", uniqueFileName);

            List<ContentModel> contents = new List<ContentModel>();

            using (var db = new ContentContext())
            {

   

                contents = db.Content.ToList();
                var content = db.Content.Where(u => u.Id == id).FirstOrDefault();
                content.Text = text;
                content.Text2 = text2;
                content.PhotoName = relativeFilePath;

           

                db.SaveChanges();

                ViewBag.contents = contents;
            }
        }
        else
        {
            List<ContentModel> contents = new List<ContentModel>();

            using (var db = new ContentContext())
            {

            

                contents = db.Content.ToList();
                var content = db.Content.Where(u => u.Id == id).FirstOrDefault();
                content.Text = text;
                content.Text2 = text2;
               

                db.SaveChanges();

                ViewBag.contents = contents;
            }
        }
      

        
        return View("Index");
    }

    [HttpPost]
    public IActionResult AdminEdit(int idNumber)
    {
        
        ViewBag.idNumber = idNumber;

        List<ContentModel> contents = new List<ContentModel>();

        using (var db = new ContentContext())
        {
            contents = db.Content.ToList();
            ViewBag.contents = contents;

            return View("Edit");
        }

        
    }

    

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


}

