using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AbeerRestaurant.Data;
using AbeerRestaurant.Models;
using Microsoft.AspNetCore.Identity;
using System;

namespace AbeerRestaurant.Pages
{
    public class CheckoutModel : PageModel
    {
        private readonly AbeerRestaurantContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CheckoutModel(AbeerRestaurantContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<CartItem> CartItems { get; set; } = new();
        public decimal TotalPrice { get; set; } = 0;

        public async Task OnGetAsync()
        {
            LoadCart();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            LoadCart(); 

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            if (CartItems.Count == 0) 
            {
                return RedirectToPage("/Menu/View");
            }

            var order = new Order
            {
                UserId = user.Id,
                Items = JsonSerializer.Serialize(CartItems.Select(c => new
                {
                    FoodItemId = c.FoodItem.ID,  
                    Name = c.FoodItem.Item_name, 
                    Price = c.FoodItem.Price,   
                    Quantity = c.Quantity
                }).ToList()), 
                TotalPrice = CartItems.Sum(c => c.FoodItem.Price.GetValueOrDefault() * c.Quantity),
                OrderDate = DateTime.Now
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            ClearCart();

            return RedirectToPage("/Index");
        }

        private void LoadCart()
        {
            var cart = HttpContext.Session.GetString("Cart");

            Console.WriteLine("Cart Data from Session: " + (cart ?? "EMPTY")); 

            var cartDict = cart == null ? new Dictionary<int, int>() : JsonSerializer.Deserialize<Dictionary<int, int>>(cart);

            CartItems = cartDict.Select(c => new CartItem
            {
                FoodItem = _context.FoodItem.Find(c.Key),
                Quantity = c.Value
            }).Where(c => c.FoodItem != null).ToList();

            Console.WriteLine("Cart Items Loaded: " + CartItems.Count); 

            TotalPrice = CartItems.Sum(item => item.FoodItem.Price.GetValueOrDefault() * item.Quantity);
        }

        private void ClearCart()
        {
            HttpContext.Session.Remove("Cart");
        }
    }

    public class CartItem
    {
        public FoodItem FoodItem { get; set; }
        public int Quantity { get; set; }
    }
}
