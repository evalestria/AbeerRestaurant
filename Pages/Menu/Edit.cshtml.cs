using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AbeerRestaurant.Data;
using AbeerRestaurant.Models;
using Microsoft.AspNetCore.Hosting;

namespace AbeerRestaurant.Pages.Menu
{
    public class EditModel : PageModel
    {
        private readonly AbeerRestaurantContext _context;
        private readonly IWebHostEnvironment _environment;

        public EditModel(AbeerRestaurantContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [BindProperty]
        public FoodItem FoodItem { get; set; } = default!;

        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fooditem = await _context.FoodItem.FirstOrDefaultAsync(m => m.ID == id);
            if (fooditem == null)
            {
                return NotFound();
            }

            FoodItem = fooditem;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var foodItem = await _context.FoodItem.FindAsync(FoodItem.ID);
            if (foodItem == null)
            {
                return NotFound();
            }

            // ✅ Update text fields
            foodItem.Item_name = FoodItem.Item_name;
            foodItem.Item_desc = FoodItem.Item_desc;
            foodItem.Price = FoodItem.Price;
            foodItem.Available = FoodItem.Available;
            foodItem.Vegetarian = FoodItem.Vegetarian;

            // ✅ Handle Image Upload
            if (ImageFile != null)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{Guid.NewGuid()}_{ImageFile.FileName}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                // ✅ Update ImageUrl
                foodItem.ImageUrl = fileName;
            }

            _context.Update(foodItem);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
