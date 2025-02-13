using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AbeerRestaurant.Data;
using AbeerRestaurant.Models;

namespace AbeerRestaurant.Pages.Menu
{
    public class ViewModel : PageModel
    {
        private readonly AbeerRestaurantContext _context;

        public ViewModel(AbeerRestaurantContext context)
        {
            _context = context;
        }

        public IList<FoodItem> FoodItem { get; set; } = default!;

        public async Task OnGetAsync()
        {
            FoodItem = await _context.FoodItem
                                     .Where(f => f.Available) // Only show available items
                                     .ToListAsync();
        }
    }
}
