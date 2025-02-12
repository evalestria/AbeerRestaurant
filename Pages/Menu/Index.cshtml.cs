using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AbeerRestaurant.Data;
using AbeerRestaurant.Models;
using Microsoft.AspNetCore.Authorization;

namespace AbeerRestaurant.Pages.Menu
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly AbeerRestaurant.Data.AbeerRestaurantContext _context;

        public IndexModel(AbeerRestaurant.Data.AbeerRestaurantContext context)
        {
            _context = context;
        }

        public IList<FoodItem> FoodItem { get;set; } = default!;

        public async Task OnGetAsync()
        {
            FoodItem = await _context.FoodItem.ToListAsync();
        }
    }
}
