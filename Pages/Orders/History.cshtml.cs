using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AbeerRestaurant.Data;
using AbeerRestaurant.Models;

namespace AbeerRestaurant.Pages.Orders
{
    public class HistoryModel : PageModel
    {
        private readonly AbeerRestaurantContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public HistoryModel(AbeerRestaurantContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<Order> Orders { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            Orders = await _context.Orders
                .Where(o => o.UserId == user.Id)
                .OrderByDescending(o => o.OrderDate) // Show latest orders first
                .ToListAsync();

            return Page();
        }
    }
}
