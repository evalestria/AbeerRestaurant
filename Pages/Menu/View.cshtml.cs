using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
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
        [BindProperty(SupportsGet = true)]
        public string SearchQuery { get; set; } = string.Empty;

        public async Task OnGetAsync()
        {
            IQueryable<FoodItem> query = _context.FoodItem.Where(f => f.Available);

            if (!string.IsNullOrEmpty(SearchQuery))
            {
                query = query.Where(f => f.Item_name.Contains(SearchQuery) || f.Item_desc.Contains(SearchQuery));
            }

            FoodItem = await query.ToListAsync();
        }
    }
}
