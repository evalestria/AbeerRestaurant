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

        [BindProperty]
        public Dictionary<int, int> Quantities { get; set; } = new(); // Stores updated quantities

        public async Task OnGetAsync()
        {
            LoadCart();
        }

        public async Task<IActionResult> OnPostUpdateAsync(int itemId)
        {
            LoadCart(); // Load cart before updating

            if (Quantities.ContainsKey(itemId) && Quantities[itemId] > 0)
            {
                var cart = GetCartDictionary();
                cart[itemId] = Quantities[itemId]; // Update quantity
                SaveCart(cart);
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRemoveAsync(int itemId)
        {
            LoadCart(); // Load cart before modifying

            var cart = GetCartDictionary();
            if (cart.ContainsKey(itemId))
            {
                cart.Remove(itemId); // Remove item from cart
            }
            SaveCart(cart);

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostCompleteAsync()
        {
            LoadCart(); // Ensure cart is loaded before saving order

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            if (CartItems.Count == 0) // Prevent saving an empty order
            {
                return RedirectToPage("/Menu/View");
            }

            var order = new Order
            {
                UserId = user.Id,
                Items = JsonSerializer.Serialize(CartItems.Select(c => new
                {
                    FoodItemId = c.FoodItem.ID,  // Store FoodItem ID
                    Name = c.FoodItem.Item_name, // Store name for reference
                    Price = c.FoodItem.Price,    // Store price at time of purchase
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
            var cart = GetCartDictionary();
            CartItems = cart.Select(c => new CartItem
            {
                FoodItem = _context.FoodItem.Find(c.Key),
                Quantity = c.Value
            }).Where(c => c.FoodItem != null).ToList();

            TotalPrice = CartItems.Sum(item => item.FoodItem.Price.GetValueOrDefault() * item.Quantity);
        }

        private Dictionary<int, int> GetCartDictionary()
        {
            var cart = HttpContext.Session.GetString("Cart");
            return cart == null ? new Dictionary<int, int>() : JsonSerializer.Deserialize<Dictionary<int, int>>(cart);
        }

        private void SaveCart(Dictionary<int, int> cart)
        {
            HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cart));
        }

        private void ClearCart()
        {
            HttpContext.Session.Remove("Cart");
        }

        public IActionResult OnGetCartTotal()
        {
            LoadCart();
            return Content(TotalPrice.ToString());
        }
    }

    public class CartItem
    {
        public FoodItem FoodItem { get; set; }
        public int Quantity { get; set; }
    }

}
