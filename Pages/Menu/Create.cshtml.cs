using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AbeerRestaurant.Data;
using AbeerRestaurant.Models;
using Microsoft.AspNetCore.Hosting;

namespace AbeerRestaurant.Pages.Menu
{
    public class CreateModel : PageModel
    {
        private readonly AbeerRestaurantContext _context;
        private readonly IWebHostEnvironment _environment;

        public CreateModel(AbeerRestaurantContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [BindProperty]
        public FoodItem FoodItem { get; set; } = default!;

        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // ✅ Handle Image Upload
            string imageUrl = "default.png"; // Store only the filename

            if (ImageFile != null)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(ImageFile.FileName)}"; // Generate unique name
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                imageUrl = fileName; // ✅ Store only filename, not full path
            }

            FoodItem.ImageUrl = imageUrl;

            _context.FoodItem.Add(FoodItem);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
