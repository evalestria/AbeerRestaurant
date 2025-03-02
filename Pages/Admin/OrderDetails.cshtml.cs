using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AbeerRestaurant.Data;
using AbeerRestaurant.Models;

namespace AbeerRestaurant.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class OrderDetailsModel : PageModel
    {
        private readonly AbeerRestaurantContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public OrderDetailsModel(AbeerRestaurantContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Order Order { get; set; }
        public string UserEmail { get; set; } = "Unknown";
        public List<CartItem> OrderItems { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Order = await _context.Orders.FindAsync(id);

            if (Order == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(Order.UserId);
            UserEmail = user?.Email ?? "Unknown";

            var rawItems = JsonSerializer.Deserialize<List<StoredOrderItem>>(Order.Items);

            OrderItems = rawItems.Select(c => new CartItem
            {
                FoodItem = _context.FoodItem.FirstOrDefault(f => f.ID == c.FoodItemId), // Now strongly typed!
                Quantity = c.Quantity
            }).Where(c => c.FoodItem != null).ToList();




            return Page();
        }
    }

    public class CartItem
    {
        public FoodItem FoodItem { get; set; }
        public int Quantity { get; set; }
    }
    public class StoredOrderItem
    {
        public int FoodItemId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }

}
