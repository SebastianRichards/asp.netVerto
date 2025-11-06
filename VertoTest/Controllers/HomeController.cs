using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using VertoTest.Models;

namespace VertoTest.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IWebHostEnvironment _environment;
    private readonly ContentContext _context;

    public HomeController(ILogger<HomeController> logger, IWebHostEnvironment environment, ContentContext context)
    {
        _logger = logger;
        _environment = environment;
        _context = context;
    }

    
    public IActionResult Index()
    {
        List<ContentModel> contents = _context.Content.ToList();
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
        _context.Add(entry);
        _context.SaveChanges();
        return Redirect("/verto/Home/Index");
    }




    public IActionResult AdminView()
    {
        List<ContentModel> contents = _context.Content.ToList();
        ViewBag.contents = contents;
        return View();
    }

    [HttpPost]
    public IActionResult AdminView(ContentModel entry)
    {
        _context.Add(entry);
        _context.SaveChanges();
        return View();
    }


    
    [HttpPost]
    public async Task<IActionResult> SubmitChanges(string text, int id, string text2, IFormFile file)
    {
        try
        {
            // Ensure text2 is never null - convert null to empty string
            text2 = text2 ?? "";
            text = text ?? "";
            
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

            List<ContentModel> contents = _context.Content.ToList();
            var content = _context.Content.Where(u => u.Id == id).FirstOrDefault();
            
            if (content != null)
            {
                content.Text = text;
                content.Text2 = text2;
                if (!string.IsNullOrEmpty(relativeFilePath))
                {
                    content.PhotoName = relativeFilePath;
                }

                _context.SaveChanges();
            }

            ViewBag.contents = contents;
        }
        else
        {
            List<ContentModel> contents = _context.Content.ToList();
            var content = _context.Content.Where(u => u.Id == id).FirstOrDefault();
            
            if (content != null)
            {
                content.Text = text;
                content.Text2 = text2;
            
                _context.SaveChanges();
            }

            ViewBag.contents = contents;
        }
        
        TempData["SuccessMessage"] = "Changes saved successfully!";
        return Redirect("/verto/Home/Index");
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error saving changes for content ID: {ContentId}", id);
        TempData["ErrorMessage"] = "An error occurred while saving changes: " + ex.Message;
        return Redirect("/verto/Home/AdminView");
    }
}

    [HttpPost]
    public IActionResult AdminEdit(int idNumber)
    {
        try
        {
            // Debug logging
            _logger.LogInformation($"AdminEdit called with idNumber: {idNumber}");
            
            ViewBag.idNumber = idNumber;

            List<ContentModel> contents = _context.Content.ToList();
            _logger.LogInformation($"Found {contents.Count} content items in database");
            
            ViewBag.contents = contents;

            // Check if the requested content exists
            var requestedContent = contents.FirstOrDefault(c => c.Id == idNumber);
            if (requestedContent == null && contents.Any())
            {
                _logger.LogWarning($"Content with ID {idNumber} not found");
                TempData["ErrorMessage"] = $"Content with ID {idNumber} not found.";
                return Redirect("/verto/Home/AdminView");
            }

            _logger.LogInformation($"Rendering Edit view for content ID: {idNumber}");
            return View("Edit");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in AdminEdit for idNumber: {idNumber}");
            TempData["ErrorMessage"] = "An error occurred while loading the edit page: " + ex.Message;
            return Redirect("/verto/Home/AdminView");
        }
    }

    [HttpPost]
    public IActionResult ResetToDefault()
    {
        // Get all content items
        var allContent = _context.Content.ToList();

            // Define default content for each text box
            var defaultContent = new Dictionary<string, (string text, string text2, string photoName)>
            {
                { "text-box-1", ("Discover our latest products and innovations in optical technology.", "", "static/products-image.jpeg") },
                { "text-box-2", ("Join us at upcoming field events and demonstrations.", "", "static/field-events-image.jpeg") },
                { "text-box-3", ("Stay updated with the latest news and product reviews.", "", "static/latest-news-image.jpeg") },
                { "text-box-4", ("Browse our gallery of stunning optical images.", "", "static/gallery-image.jpeg") },
                { "text-box-5", ("Special discount on premium binoculars this month.", "Save 20%", "static/row2image1.png") },
                { "text-box-6", ("Free shipping on orders over $200.", "Limited Time", "static/row2image2.png") },
                { "text-box-7", ("Exclusive bundle deals for professionals.", "Professional Series", "static/row2image3.png") },
                { "text-box-8", ("Binoculars", "", "static/bin-1.jpeg") },
                { "text-box-9", ("Telescopes", "", "static/bin-2.jpeg") },
                { "text-box-10", ("Spotting Scopes", "", "static/bin-3.jpeg") },
                { "text-box-11", ("Accessories", "", "static/bin-4.jpeg") }
            };

            // Reset each content item to default values
            foreach (var content in allContent)
            {
                if (defaultContent.ContainsKey(content.Name))
                {
                    var defaultValues = defaultContent[content.Name];
                    content.Text = defaultValues.text;
                    content.Text2 = defaultValues.text2;
                    content.PhotoName = defaultValues.photoName;
                }
            }

            // Save changes to database
            _context.SaveChanges();

        // Redirect back to admin view with success message
        TempData["SuccessMessage"] = "All content has been reset to default values successfully!";
        return Redirect("/verto/Home/AdminView");
    }

    [HttpPost]
    public IActionResult SeedInitialData()
    {
        // Check if data already exists
        if (_context.Content.Any())
        {
            TempData["InfoMessage"] = "Database already contains content. Use 'Reset to Default' to restore default values.";
            return Redirect("/verto/Home/AdminView");
        }

            // Create initial content items
            var initialContent = new List<ContentModel>
            {
                new ContentModel { Name = "text-box-1", Text = "Discover our latest products and innovations in optical technology.", Text2 = "", PhotoName = "static/products-image.jpeg" },
                new ContentModel { Name = "text-box-2", Text = "Join us at upcoming field events and demonstrations.", Text2 = "", PhotoName = "static/field-events-image.jpeg" },
                new ContentModel { Name = "text-box-3", Text = "Stay updated with the latest news and product reviews.", Text2 = "", PhotoName = "static/latest-news-image.jpeg" },
                new ContentModel { Name = "text-box-4", Text = "Browse our gallery of stunning optical images.", Text2 = "", PhotoName = "static/gallery-image.jpeg" },
                new ContentModel { Name = "text-box-5", Text = "Special discount on premium binoculars this month.", Text2 = "Save 20%", PhotoName = "static/row2image1.png" },
                new ContentModel { Name = "text-box-6", Text = "Free shipping on orders over $200.", Text2 = "Limited Time", PhotoName = "static/row2image2.png" },
                new ContentModel { Name = "text-box-7", Text = "Exclusive bundle deals for professionals.", Text2 = "Professional Series", PhotoName = "static/row2image3.png" },
                new ContentModel { Name = "text-box-8", Text = "Binoculars", Text2 = "", PhotoName = "static/bin-1.jpeg" },
                new ContentModel { Name = "text-box-9", Text = "Telescopes", Text2 = "", PhotoName = "static/bin-2.jpeg" },
                new ContentModel { Name = "text-box-10", Text = "Spotting Scopes", Text2 = "", PhotoName = "static/bin-3.jpeg" },
                new ContentModel { Name = "text-box-11", Text = "Accessories", Text2 = "", PhotoName = "static/bin-4.jpeg" }
            };

        // Add all content to database
        _context.Content.AddRange(initialContent);
        _context.SaveChanges();

        TempData["SuccessMessage"] = "Initial content has been created successfully!";
        return Redirect("/verto/Home/AdminView");
    }

    

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


}

